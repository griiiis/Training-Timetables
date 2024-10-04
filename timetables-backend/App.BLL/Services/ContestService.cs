using App.BLL.DTO;
using App.BLL.DTO.DTOs.Contests;
using App.BLL.DTO.Models;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using ContestRole = App.DAL.DTO.ContestRole;

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

    public async Task<EditContestDTO> GetContestEditModel(Guid userId, Guid contestId)
    {
        var contestInfo = await FirstOrDefaultAsync(contestId, userId);
        
        var previousLevels = contestInfo!.ContestLevels
            .Select(e => e.Level!.Id).ToList();

        var previousPackages = contestInfo.ContestPackages
            .Select(e => e.PackageGameTypeTime!.Id).ToList();

        var previousTimes = contestInfo.ContestTimes
            .Select(e => e.Time!.Id).ToList();

        var vm = new EditContestDTO
        {
            Id = contestInfo.Id,
            ContestName = contestInfo.ContestName,
            Description = contestInfo.Description,
            TotalHours = contestInfo.TotalHours,
            From = contestInfo.From,
            Until = contestInfo.Until,
            LocationId = contestInfo.LocationId,
            ContestTypeId = contestInfo.ContestTypeId,
            LevelIds = previousLevels,
            PackagesIds = previousPackages!,
            TimesIds = previousTimes!,
        };
        return vm;
    }

    public async Task PutContest(Guid userId, Guid id, EditContestDTO contestDTO)
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

        foreach (var packageId in contestDTO.PackagesIds!)
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
                ContestId = contestDTO.Id,
                GameTypeId = gameTypeId
            };
            Uow.ContestGameTypes.Add(contestGameType);
        }

        foreach (var packageId in contestDTO.PackagesIds!)
        {
            var package = new App.DAL.DTO.ContestPackage()
            {
                ContestId = contestDTO.Id,
                PackageGameTypeTimeId = packageId
            };
            Uow.ContestPackages.Add(package);
        }

        foreach (var timeId in contestDTO.TimesIds!)
        {
            var times = new App.DAL.DTO.ContestTime()
            {
                ContestId = contestDTO.Id,
                TimeId = timeId
            };
            Uow.ContestTimes.Add(times);
        }

        foreach (var levelId in contestDTO.LevelIds!)
        {
            var contestLevel = new App.DAL.DTO.ContestLevel()
            {
                ContestId = contestDTO.Id,
                LevelId = levelId
            };
            Uow.ContestLevels.Add(contestLevel);
        }

        var contest = new Contest
        {
            Id = contestDTO.Id,
            ContestName = contestDTO.ContestName,
            Description = contestDTO.Description,
            From = contestDTO.From,
            Until = contestDTO.Until,
            TotalHours = contestDTO.TotalHours,
            ContestTypeId = contestDTO.ContestTypeId,
            LocationId = contestDTO.LocationId,
        };

        UpdateContestWithUser(userId, contest);
        await Uow.SaveChangesAsync();
    }

    public async Task<Contest> PostContest(Guid userId, CreateContestDTO createContestDto)
    {
        var contestId = Guid.NewGuid();
        createContestDto.From = createContestDto.From.ToUniversalTime();
        createContestDto.Until = createContestDto.Until.ToUniversalTime();

        foreach (var levelId in createContestDto.SelectedLevelIds!)
        {
            var contestLevel = new App.DAL.DTO.ContestLevel
            {
                ContestId = contestId,
                LevelId = levelId
            };
            Uow.ContestLevels.Add(contestLevel);
        }

        var gameTypes = new HashSet<Guid>();
        var allPackages = (await Uow.PackageGameTypeTimes.GetAllAsync(default)).ToList();

        foreach (var id in createContestDto.SelectedPackagesIds!)
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
                ContestId = contestId,
                GameTypeId = gameTypeId
            };
            Uow.ContestGameTypes.Add(contestGameType);
        }

        foreach (var timeId in createContestDto.SelectedTimesIds!)
        {
            var timeOfDay = new App.DAL.DTO.ContestTime()
            {
                ContestId = contestId,
                TimeId = timeId
            };
            Uow.ContestTimes.Add(timeOfDay);
        }

        foreach (var packageId in createContestDto.SelectedPackagesIds!)
        {
            var package = new App.DAL.DTO.ContestPackage()
            {
                ContestId = contestId,
                PackageGameTypeTimeId = packageId
            };
            Uow.ContestPackages.Add(package);
        }
        
        //Contest Roles by default
        var trainerRole = new ContestRole()
        {
            ContestRoleName = "Trainer",
            ContestId = contestId
        };

        var participantRole = new DAL.DTO.ContestRole()
        {
            ContestRoleName = "Participant",
            ContestId = contestId
        };
        Uow.ContestRoles.Add(participantRole);
        Uow.ContestRoles.Add(trainerRole);

        var contest = new Contest
        {
            Id = contestId,
            ContestName = createContestDto.ContestName,
            Description = createContestDto.Description,
            From = createContestDto.From,
            Until = createContestDto.Until,
            TotalHours = createContestDto.TotalHours,
            ContestTypeId = createContestDto.ContestTypeId,
            LocationId = createContestDto.LocationId,
        };


        var newContest = AddContestWithUser(userId, contest);
        await Uow.SaveChangesAsync();
        return newContest;
    }

    public async Task<IEnumerable<OwnerContestsDTO>> GetOwnerContests(Guid userId)
    {
        var list =(await GetAllAsync()).Select(e => new OwnerContestsDTO
        {
            Id = e.Id,
            ContestName = e.ContestName,
            Description = e.Description,
            TotalHours = e.TotalHours,
            From = e.From,
            Until = e.Until,
            LocationName = e.Location!.LocationName,
            ContestTypeName = e.ContestType!.ContestTypeName
        }).ToList();
        return list;
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