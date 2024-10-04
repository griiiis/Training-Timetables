using App.DAL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class GameService : BaseEntityService<App.DAL.DTO.Game, App.BLL.DTO.Game, IGameRepository, IAppUnitOfWork>, IGameService
{
    public GameService(IAppUnitOfWork uow, IGameRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Game, App.BLL.DTO.Game>(mapper))
    {
    }

    public async Task<IEnumerable<App.BLL.DTO.Game>> GetContestGamesWithoutTeachers(Guid contestId)
    {
        return (await Repository.GetContestGamesWithoutTeachers(contestId)).Select(e => Mapper.Map(e))!;
    }
    
    public async Task<IEnumerable<App.BLL.DTO.Game>> GetContestGames(Guid contestId)
    {
        return (await Repository.GetContestGames(contestId)).Select(e => Mapper.Map(e))!;
    }

    public bool AnyContestGames(Guid contestId)
    {
        return Repository.AnyContestGames(contestId);
    }

    public async Task<IEnumerable<App.BLL.DTO.Game>> GetUserContestGames(Guid contestId, Guid userId)
    {
        return (await Repository.GetUserContestGames(contestId, userId)).Select(e => Mapper.Map(e))!;
    }

    public async void CreateGames(App.BLL.DTO.Models.CreateGamesData gamesData, Guid contestId)
    {
        var timesByDay =
                gamesData.SelectedTimesIds;

            var listNotAddedTeams = new List<Team>();
            var currentlyAddedPeople = 0;
            var allTimes = Uow.Times.GetAllCurrentContestAsync(contestId).Result.ToList();
            var gameTypes = Uow.GameTypes.GetAllCurrentContestAsync(contestId).Result.ToList();
            var trainers = (await Uow.UserContestPackages.GetContestTeachers(contestId)).ToList();
            for (var i = 0; i < gameTypes.Count; i++)
            {
                var peopleOnCourtPref = gamesData.PeoplePerCourtInputs![i];
                foreach (var day in timesByDay!)
                {
                    var trainersCounter = 0;
                    var gameTypeId = gameTypes[i].Id;

                    //Väljakute arv
                    var courtsCount = gamesData.SelectedCourtIds![i].Count;

                    //ALL this gametype teams
                    var thisGameTypeAllTeams = Uow.Teams.GetAllCurrentContestAsync(contestId).Result
                        .Where(g => g.GameTypeId == gameTypes[i].Id).ToList();

                    var peopleCount = thisGameTypeAllTeams.Count;

                    //Inimesi väljakul
                    // var playersPerCourt = Math.Ceiling((decimal)peopleCount / (day.SelectedTimesList!.Count * courtsCount));
                    var playersPerCourt = peopleOnCourtPref;

                        for (var j = 0; j < day.SelectedTimesList!.Count; j++) // kellaajad
                    {
                        int gamesCountPerTime;
                        if (peopleCount / courtsCount != playersPerCourt)
                        {
                            gamesCountPerTime =
                                (int)Math.Ceiling((decimal)peopleCount /
                                                  (day.SelectedTimesList!.Count * playersPerCourt));
                        }
                        else
                        {
                            gamesCountPerTime = (int)Math.Ceiling((decimal)peopleCount / playersPerCourt);
                            thisGameTypeAllTeams = Uow.UserContestPackages.GetContestParticipants(contestId).Result.Select(e => e.Team)
                                .Where(g => g!.GameTypeId == gameTypes[i].Id).ToList();
                        }

                        var thisLevelGameTypeAllTeams = thisGameTypeAllTeams.ToList();
                        var courtCounter = 0;

                        for (var k = 0; k < gamesCountPerTime; k++)
                        {
                            Guid courtId;
                            if (gamesData.SelectedCourtIds[i].Count > courtCounter)
                            {
                                courtId = gamesData.SelectedCourtIds[i][courtCounter++];
                            }
                            else
                            {
                                throw new Exception("Nii palju väljakuid pole!");
                            }

                            var currentTime = allTimes.Find(e =>
                                e.Id == day.SelectedTimesList[j]);

                            var currentDay = day.Date;

                            var game = new Game
                            {
                                Title = "Selle anname ise",
                                From = currentDay!.Value.ToDateTime(currentTime!.From),
                                Until = currentDay.Value.ToDateTime(currentTime.Until),
                                ContestId = contestId,
                                CourtId = courtId,
                                GameTypeId = gameTypeId,
                                Id = Guid.NewGuid()
                            };
                            
                            var teamGames = new List<TeamGame>();
                            
                            //Add trainer to the game
                            if (trainersCounter == gamesData.SelectedTrainersIds![i].Count)
                            {
                                trainersCounter = 0;
                            }

                            var trainerTeamId = trainers.Where(e =>
                                e.Id.Equals(gamesData.SelectedTrainersIds[i][trainersCounter])).Select(e => e.TeamId).FirstOrDefault();
                            
                            var trainerTeamGame = new TeamGame
                            {
                                TeamId = trainerTeamId,
                                GameId = game.Id,
                            };
                            teamGames.Add(trainerTeamGame);

                            trainersCounter++;
                            thisLevelGameTypeAllTeams.AddRange(listNotAddedTeams);

                            listNotAddedTeams = new List<Team>();
                            var random = new Random();
                            //Lisame Tiimid mängu
                            thisLevelGameTypeAllTeams =
                                thisGameTypeAllTeams.OrderBy(t => t!.UserContestPackages!.Count)
                                    .ThenByDescending(e => e!.Level!.Title.ToString()).ThenBy(_ => random.Next()).ToList();


                            var levelIdsList = new List<Guid>();
                            foreach (var team in thisLevelGameTypeAllTeams)
                            {
                                var currentTeamPlayersCount = team!.UserContestPackages!.Count;
                                var playersCountThatCanBeAddedToCourt = playersPerCourt - currentlyAddedPeople;

                                if (playersCountThatCanBeAddedToCourt >= currentTeamPlayersCount)
                                {
                                    currentlyAddedPeople += currentTeamPlayersCount;
                                    var teamGame = new TeamGame
                                    {
                                        TeamId = team.Id,
                                        GameId = game.Id,
                                    };
                                    thisGameTypeAllTeams.Remove(team);

                                    levelIdsList.Add(team.LevelId);
                                    teamGames.Add(teamGame);
                                }
                                else
                                {
                                    listNotAddedTeams.Add(team);
                                }

                                if (currentlyAddedPeople == playersPerCourt ||
                                    currentlyAddedPeople == thisLevelGameTypeAllTeams.Count)
                                {
                                    var levelId = levelIdsList
                                        .GroupBy(x => x)
                                        .OrderByDescending(g => g.Count())
                                        .Select(g => g.Key)
                                        .FirstOrDefault();
                                    game.LevelId = levelId;
                                    Uow.Games.Add(game);

                                    foreach (var teamGame in teamGames)
                                    {
                                        Uow.TeamGames.Add(teamGame);
                                    }

                                    currentlyAddedPeople = 0;
                                }
                                else
                                {
                                    continue;
                                }

                                break;
                            }
                        }
                    }
                }
            }
    }
}