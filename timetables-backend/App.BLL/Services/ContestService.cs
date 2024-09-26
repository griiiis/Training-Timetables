using App.BLL.DTO;
using App.BLL.DTO.Models;
using App.BLL.DTO.Models.Contests;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class ContestService :
    BaseEntityService<App.DAL.DTO.Contest, App.BLL.DTO.Contest, IContestRepository, IAppUnitOfWork>, IContestService
{
    private readonly BLLDalMapper<App.DAL.DTO.Level, Level> _levelMapper;
    private readonly BLLDalMapper<App.DAL.DTO.PackageGameTypeTime, PackageGameTypeTime> _packagesMapper;
    private readonly BLLDalMapper<App.DAL.DTO.Time, Time> _timesMapper;
    private readonly BLLDalMapper<App.DAL.DTO.Location, Location> _locationMapper;

    public ContestService(IAppUnitOfWork uow, IContestRepository repository, IMapper mapper)
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Contest, App.BLL.DTO.Contest>(mapper))
    {
        _levelMapper = new BLLDalMapper<App.DAL.DTO.Level, Level>(mapper);
        _packagesMapper = new BLLDalMapper<App.DAL.DTO.PackageGameTypeTime, PackageGameTypeTime>(mapper);
        _timesMapper = new BLLDalMapper<App.DAL.DTO.Time, Time>(mapper);
        _locationMapper = new BLLDalMapper<App.DAL.DTO.Location, Location>(mapper);
    }

    public async Task<App.BLL.DTO.Contest?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(id, userId));
    }

    public new async Task<IEnumerable<App.BLL.DTO.Contest>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId)).Select(de => Mapper.Map(de));
    }

    public async Task<IEnumerable<Contest>> GetUserContests(Guid userId)
    {
        return (await Repository.GetUserContests(userId)).Select(de => Mapper.Map(de));
    }

    public async Task<ContestEditModel> GetContestEditModel(Guid userId, Guid contestId)
    {
        var contestInfo = await FirstOrDefaultAsync(contestId, userId);
        
        var previousLevels = contestInfo!.ContestLevels
            .Select(e => e.Level!.Id).ToList();

        var previousPackages = contestInfo.ContestPackages
            .Select(e => e.PackageGameTypeTime!.Id).ToList();

        var previousTimes = contestInfo.ContestTimes
            .Select(e => e.Time!.Id).ToList();

        var vm = new ContestEditModel()
        {
            Contest = contestInfo,
            LevelIds = previousLevels,
            PackagesIds = previousPackages!,
            TimesIds = previousTimes!,
        };
        return vm;
    }

    public async Task PutContest(Guid userId, Guid id, ContestEditModel contest)
    {
        //Remove previous packages
        var previousPackages = Uow.ContestPackages.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
        foreach (var package in previousPackages)
        {
            await Uow.ContestPackages.RemoveAsync(package);
        }

        //Remove previous gameTypes
        var previousGameTypes =
            Uow.ContestGameTypes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
        foreach (var gameType in previousGameTypes)
        {
            await Uow.ContestGameTypes.RemoveAsync(gameType);
        }

        //Remove times
        var previousTimes = Uow.ContestTimes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
        foreach (var time in previousTimes)
        {
            await Uow.ContestTimes.RemoveAsync(time);
        }

        //Remove levels
        var previousLevels = Uow.ContestLevels.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
        foreach (var level in previousLevels)
        {
            await Uow.ContestLevels.RemoveAsync(level);
        }

        var gameTypes = new HashSet<Guid>();
        var allPackages = (await Uow.PackageGameTypeTimes.GetAllAsync(default)).ToList();

        foreach (var packageId in contest.PackagesIds!)
        {
            var gameTypeId = allPackages
                .FirstOrDefault(e => e.Id.Equals(packageId) && !gameTypes.Contains(e.GameTypeId))
                ?.GameTypeId;

            if (gameTypeId != null)
            {
                gameTypes.Add(gameTypeId.Value);
            }
        }

        foreach (var gameTypeId in gameTypes)
        {
            var contestGameType = new App.DAL.DTO.ContestGameType()
            {
                ContestId = contest.Contest.Id,
                GameTypeId = gameTypeId
            };
            Uow.ContestGameTypes.Add(contestGameType);
        }

        foreach (var packageId in contest.PackagesIds!)
        {
            var package = new App.DAL.DTO.ContestPackage()
            {
                ContestId = contest.Contest.Id,
                PackageGameTypeTimeId = packageId
            };
            Uow.ContestPackages.Add(package);
        }

        foreach (var timeId in contest.TimesIds!)
        {
            var times = new App.DAL.DTO.ContestTime()
            {
                ContestId = contest.Contest.Id,
                TimeId = timeId
            };
            Uow.ContestTimes.Add(times);
        }

        foreach (var levelId in contest.LevelIds!)
        {
            var contestLevel = new App.DAL.DTO.ContestLevel()
            {
                ContestId = contest.Contest.Id,
                LevelId = levelId
            };
            Uow.ContestLevels.Add(contestLevel);
        }


        UpdateContestWithUser(userId, contest.Contest);
        await Uow.SaveChangesAsync();
    }

    public async Task<Contest> PostContest(Guid userId, ContestCreateModel contest)
    {
        contest.Contest.Id = Guid.NewGuid();
        contest.Contest.From = contest.Contest.From.ToUniversalTime();
        contest.Contest.Until = contest.Contest.Until.ToUniversalTime();

        foreach (var levelId in contest.SelectedLevelIds!)
        {
            var contestLevel = new App.DAL.DTO.ContestLevel
            {
                ContestId = contest.Contest.Id,
                LevelId = levelId
            };
            Uow.ContestLevels.Add(contestLevel);
        }

        var gameTypes = new HashSet<Guid>();
        var allPackages = (await Uow.PackageGameTypeTimes.GetAllAsync(default)).ToList();

        foreach (var id in contest.SelectedPackagesIds!)
        {
            var gameTypeId = allPackages
                .FirstOrDefault(e => e.Id.Equals(id) && !gameTypes.Contains(e.GameTypeId))
                ?.GameTypeId;

            if (gameTypeId != null)
            {
                gameTypes.Add(gameTypeId.Value);
            }
        }

        foreach (var gameTypeId in gameTypes)
        {
            var contestGameType = new App.DAL.DTO.ContestGameType()
            {
                ContestId = contest.Contest.Id,
                GameTypeId = gameTypeId
            };
            Uow.ContestGameTypes.Add(contestGameType);
        }

        foreach (var timeId in contest.SelectedTimesIds!)
        {
            var timeOfDay = new App.DAL.DTO.ContestTime()
            {
                ContestId = contest.Contest.Id,
                TimeId = timeId
            };
            Uow.ContestTimes.Add(timeOfDay);
        }

        foreach (var packageId in contest.SelectedPackagesIds!)
        {
            var package = new App.DAL.DTO.ContestPackage()
            {
                ContestId = contest.Contest.Id,
                PackageGameTypeTimeId = packageId
            };
            Uow.ContestPackages.Add(package);
        }

        var newContest = AddContestWithUser(userId, contest.Contest);
        await Uow.SaveChangesAsync();
        return newContest;
    }

    public App.BLL.DTO.Contest AddContestWithUser(Guid userId, App.BLL.DTO.Contest contest)
    {
        var dto = Mapper.Map(contest)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsContestOwnedByUser(Guid userId, Guid contestId)
    {
        return Repository.FirstOrDefault(contestId)?.AppUserId == userId;
    }

    public Contest UpdateContestWithUser(Guid userId, Contest contest)
    {
        var dto = Mapper.Map(contest)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}