using App.Contracts.BLL;
using App.BLL.DTO;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Areas.ContestAdmin.ViewModels;

namespace WebApp.Areas.ContestAdmin.Controllers
{
    [Authorize(Roles = "Contest Admin")]
    [Area("ContestAdmin")]
    public class GamesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public GamesController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: Games
        public async Task<IActionResult> Index(Guid contestId)
        {
            var contest = _bll.Contests.FirstOrDefaultAsync(contestId).Result!;
            var teachers = (await _bll.UserContestPackages.GetContestTeachers(contestId)).ToList();
            var allDays = new List<DateTime>();
            for (var date = contest.From.Date; date <= contest.Until.Date; date = date.AddDays(1))
            {
                allDays.Add(date);
            }

            var vm = new GameIndexViewModel
            {
                Games = (await _bll.Games.GetContestGamesWithoutTeachers(contestId)).ToList(),
                Contest = contest,
                GameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(contestId)).ToList(),
                Days = allDays,
                Times = (await _bll.Times.GetAllCurrentContestAsync(contestId)).ToList(),
                Teachers = teachers,
                TeacherIds = teachers.Select(e => e.AppUserId).ToList()
            };
            return View(vm);
        }

        // GET: Games/Create
        public async Task<IActionResult> Create(Guid contestId)
        {
            var contest = _bll.Contests.FirstOrDefaultAsync(contestId).Result!;
            var allUsersWithoutTrainers =
                (await _bll.UserContestPackages.GetContestParticipants(contestId)).ToList();
            var dictionary = new Dictionary<Guid, int>();
            foreach (var gameType in _bll.GameTypes.GetAllCurrentContestAsync(contestId).Result.ToList())
            {
                dictionary.Add(gameType.Id, allUsersWithoutTrainers.Count(u => u.Team!.GameTypeId == gameType.Id));
            }

            var vm = new GameCreateEditViewModel
            {
                Teams = _bll.Teams.GetAllCurrentContestAsync(contest.Id).Result.ToList(),
                UsersWithoutTrainers = allUsersWithoutTrainers,
                Trainers = (await _bll.UserContestPackages.GetContestTeachers(contestId)).ToList(),
                Times = _bll.Times.GetAllCurrentContestAsync(contestId).Result.ToList(),
                Contest = contest,
                GameTypes = _bll.GameTypes.GetAllCurrentContestAsync(contestId).Result.ToList(),
                LevelsSelectList = _bll.Levels.GetAllCurrentContestAsync(contestId).Result.Select(l =>
                    new SelectListItem { Text = l.Title, Value = l.Id.ToString(), Selected = true }).ToList(),
                Courts = (await _bll.Courts.GetAllCurrentContestAsync(contestId)).ToList(),
                PeopleCountPerGameType = dictionary,
            };
            return View(vm);
        }

        private List<Team> _thisGameTypeAllTeams = new List<Team>();
        private List<Team> _thisLevelGameTypeAllTeams = new List<Team>();
        private List<Team> _listNotAddedTeams = new List<Team>();
        private List<TeamGame> _teamsInGame = new List<TeamGame>();
        private List<Guid> _levelIdsList = new List<Guid>();
        private List<Time> _allTimes = new List<Time>();
        private List<UserContestPackage> _trainers = new List<UserContestPackage>();
        private List<TimeOfDaysViewModel> _timeOfDaysVm = new List<TimeOfDaysViewModel>();
        private GameTypeViewModel _gameTypeViewModel = new GameTypeViewModel();
        private TimeOfDaysViewModel _timeOfDayVm = new TimeOfDaysViewModel();
        private Time? _currentTime;
        private Guid _contestId;
        private int _playersPerCourt;
        private int _currentlyAddedPeople;
        private int _courtCounter;
        private int _trainersCounter;
        private int _courtsCount;
        private int _peopleCount;

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameCreateEditViewModel vm)
        {
            _contestId = vm.Contest.Id;
            _allTimes = _bll.Times.GetAllCurrentContestAsync(_contestId).Result.ToList();
            _trainers = (await _bll.UserContestPackages.GetContestTeachers(_contestId)).ToList();
            _timeOfDaysVm = JsonConvert.DeserializeObject<Dictionary<string, List<TimeOfDaysViewModel>>>(vm.SelectedTimes!)!
                .SelectMany(item => item.Value).ToList();
            PerGameType(vm);
            await _bll.SaveChangesAsync();
            return RedirectToAction("Index", "Contests");
        }

        public void PerGameType(GameCreateEditViewModel vm)
        {
            foreach (var gameTypeViewModel in vm.GameTypesModel)
            {
                _gameTypeViewModel = gameTypeViewModel;
                _courtsCount = gameTypeViewModel.SelectedCourtsList!.Count;
                
                //ALL current gametype teams
                _thisGameTypeAllTeams = _bll.Teams.GetAllCurrentContestAsync(_contestId).Result
                    .Where(g => g.GameTypeId == gameTypeViewModel.GameTypeId).ToList();
                
                //Total count for that gameType
                _peopleCount = _thisGameTypeAllTeams.Count;
                
                //Players per court
                _playersPerCourt = gameTypeViewModel.PeopleOnCourt;
                PerDay();
            }
        }

        public void PerDay()
        {
            foreach (var timeOfDayVm in _timeOfDaysVm)
            {
                _timeOfDayVm = timeOfDayVm;
                _trainersCounter = 0;
                PerTimeOfDay();

                if (_thisLevelGameTypeAllTeams is { Count: > 0 })
                {
                    _listNotAddedTeams.AddRange(_thisLevelGameTypeAllTeams);
                }
            }
        }

        public void PerTimeOfDay()
        {
            for (var j = 0; j < _timeOfDayVm.SelectedTimesList!.Count; j++) // kellaajad
            { 
                //Get as Time Entity
                _currentTime = _allTimes.Find(e =>
                    e.Id == _timeOfDayVm.SelectedTimesList![j]);
                
                PerCourt();
            }
        }

        public void PerCourt()
        {
            _courtCounter = 0;
            for (var k = 0; k < _courtsCount; k++)
            {
                newGameAndTrainers(_gameTypeViewModel.SelectedCourtsList![_courtCounter++]);
            }
        }

        public void newGameAndTrainers(Guid courtId)
        {
            var game = createNewGame(courtId);
            preAddingTeamsToGames(game);
        }

        public Game createNewGame(Guid courtId)
        {
            _teamsInGame = new List<TeamGame>();
            var game = new Game
            {
                Title = "Game",
                From = _timeOfDayVm.Date!.Value.ToDateTime(_currentTime!.From),
                Until = _timeOfDayVm.Date.Value.ToDateTime(_currentTime.Until),
                ContestId = _contestId,
                CourtId = courtId,
                GameTypeId = _gameTypeViewModel.GameTypeId,
                Id = Guid.NewGuid()
            };
            
            addTrainerToGame(game);
            return game;
        }

        public void addTrainerToGame(Game game)
        {
            //Add trainer to the game
            if (_trainersCounter == _gameTypeViewModel.SelectedTeachersList!.Count)
            {
                _trainersCounter = 0;
            }

            var trainerId= _trainers.Where(e =>
                    e.Id.Equals(_gameTypeViewModel.SelectedTeachersList[_trainersCounter]))
                .Select(e => e.TeamId).FirstOrDefault();

            var trainer = new TeamGame
            {
                TeamId = trainerId,
                GameId = game.Id,
            };
            _teamsInGame.Add(trainer);
            _trainersCounter++;
        }

        public void preAddingTeamsToGames(Game game)
        {
            shuffleTeams();
            foreach (var team in _thisLevelGameTypeAllTeams!)
            {
                var currentTeamPlayersCount = team.UserContestPackages!.Count;
                var playersCountThatCanBeAddedToCourt = _playersPerCourt - _currentlyAddedPeople;

                if (playersCountThatCanBeAddedToCourt >= currentTeamPlayersCount)
                {
                    addTeamToGame(currentTeamPlayersCount, team, game);
                }
                else
                {
                    _listNotAddedTeams.Add(team);
                }

                if (_currentlyAddedPeople == _playersPerCourt ||
                    _currentlyAddedPeople == _thisLevelGameTypeAllTeams.Count)
                {
                    getGameLevel(game);
                }
                else
                {
                    continue;
                }

                break;
            }
        }

        public void shuffleTeams()
        {
            // Ordering Teams
            // Firstly by how many people are in the team
            // Then by their level
            // Then randomly
            
            var random = new Random();
            _thisLevelGameTypeAllTeams.AddRange(_listNotAddedTeams);
            
            _thisLevelGameTypeAllTeams =
                _thisGameTypeAllTeams.OrderBy(t => t!.UserContestPackages!.Count)
                    .ThenByDescending(e => e.Level!.Title.ToString()).ThenBy(_ => random.Next())
                    .ToList();
            _listNotAddedTeams = new List<Team>();
        }

        public void addTeamToGame(int currentTeamPlayersCount, Team team, Game game)
        {
            _levelIdsList = new List<Guid>();
            _currentlyAddedPeople += currentTeamPlayersCount;
            var teamGame = new TeamGame
            {
                TeamId = team.Id,
                GameId = game.Id,
            };
            _thisGameTypeAllTeams.Remove(team);
            _levelIdsList.Add(team.LevelId);
            _teamsInGame.Add(teamGame);
        }

        public void getGameLevel(Game game)
        {
            var levelId = _levelIdsList
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
            game.LevelId = levelId;
            _bll.Games.Add(game);
            foreach (var teamGame in _teamsInGame)
            {
                _bll.TeamGames.Add(teamGame);
            }

            _currentlyAddedPeople = 0;
        }
    }
}