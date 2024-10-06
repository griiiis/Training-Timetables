using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using App.Domain;
using App.Domain.Identity;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.AspNetCore.Identity;

namespace App.DAL.EF;

public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public AppUnitOfWork(AppDbContext dbContext, IMapper mapper, UserManager<AppUser> userManager) : base(dbContext)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    private IContestRepository? _contests;
    public IContestRepository Contests => _contests ??= new ContestRepository(UowDbContext, _mapper);
    private IEntityRepository<App.DAL.DTO.ContestLevel>? _contestLevels;

    public IEntityRepository<App.DAL.DTO.ContestLevel> ContestLevels => _contestLevels ??=
        new BaseEntityRepository<ContestLevel, App.DAL.DTO.ContestLevel, AppDbContext>(UowDbContext,
            new DalDomainMapper<ContestLevel, App.DAL.DTO.ContestLevel>(_mapper));

    private IEntityRepository<App.DAL.DTO.ContestGameType>? _contestGameTypes;

    public IEntityRepository<App.DAL.DTO.ContestGameType> ContestGameTypes => _contestGameTypes ??=
        new BaseEntityRepository<ContestGameType, App.DAL.DTO.ContestGameType, AppDbContext>(UowDbContext,
            new DalDomainMapper<ContestGameType, App.DAL.DTO.ContestGameType>(_mapper));

    private IEntityRepository<App.DAL.DTO.ContestTime>? _contestTimes;

    public IEntityRepository<App.DAL.DTO.ContestTime> ContestTimes => _contestTimes ??=
        new BaseEntityRepository<ContestTime, App.DAL.DTO.ContestTime, AppDbContext>(UowDbContext,
            new DalDomainMapper<ContestTime, App.DAL.DTO.ContestTime>(_mapper));

    private IEntityRepository<App.DAL.DTO.ContestPackage>? _contestPackages;

    public IEntityRepository<App.DAL.DTO.ContestPackage> ContestPackages => _contestPackages ??=
        new BaseEntityRepository<ContestPackage, App.DAL.DTO.ContestPackage, AppDbContext>(UowDbContext,
            new DalDomainMapper<ContestPackage, App.DAL.DTO.ContestPackage>(_mapper));

    private IAppUserRepository? _appUsers;
    public IAppUserRepository AppUsers => _appUsers ??= new AppuserRepository(UowDbContext, _mapper);

    private IContestTypeRepository? _contestTypes;
    public IContestTypeRepository ContestTypes => _contestTypes ??=
        new ContestTypeRepository(UowDbContext, _mapper);

    private ILocationRepository? _locations;

    public ILocationRepository Locations => _locations ??=
        new LocationRepository(UowDbContext, _mapper);

    private ICourtRepository? _courts;
    public ICourtRepository Courts => _courts ??= new CourtRepository(UowDbContext, _mapper);

    private IGameRepository? _games;
    public IGameRepository Games => _games ??= new GameRepository(UowDbContext, _mapper);

    private IGameTypeRepository? _gameTypes;
    public IGameTypeRepository GameTypes => _gameTypes ??= new GameTypeRepository(UowDbContext, _mapper);

    private ILevelRepository? _levels;
    public ILevelRepository Levels => _levels ??= new LevelRepository(UowDbContext, _mapper);

    private IPackageGameTypeTimeRepository? _packageGameTypeTimes;

    public IPackageGameTypeTimeRepository PackageGameTypeTimes =>
        _packageGameTypeTimes ??= new PackageGameTypeTimeRepository(UowDbContext, _mapper);

    private IRolePreferenceRepository? _rolePreferences;

    public IRolePreferenceRepository RolePreferences =>
        _rolePreferences ??= new RolePreferenceRepository(UowDbContext, _mapper);

    private ITeamGameRepository? _teamGames;
    public ITeamGameRepository TeamGames => _teamGames ??= new TeamGameRepository(UowDbContext, _mapper);

    private ITeamRepository? _teams;
    public ITeamRepository Teams => _teams ??= new TeamRepository(UowDbContext, _mapper, _userManager);

    private ITimeOfDayRepository? _timeOfDays;
    public ITimeOfDayRepository TimeOfDays => _timeOfDays ??= new TimeOfDayRepository(UowDbContext, _mapper);

    private ITimeTeamRepository? _timeTeams;
    public ITimeTeamRepository TimeTeams => _timeTeams ??= new TimeTeamRepository(UowDbContext, _mapper);

    private IUserContestPackageRepository? _userContestPackages;

    public IUserContestPackageRepository UserContestPackages =>
        _userContestPackages ??= new UserContestPackageRepository(UowDbContext, _mapper, _userManager);

    private ITimeRepository? _times;
    public ITimeRepository Times => _times ??= new TimeRepository(UowDbContext, _mapper);

    private IContestRoleRepository? _contestRoles;
    public IContestRoleRepository ContestRoles => _contestRoles ??= new ContestRoleRepository(UowDbContext, _mapper);
    
    private IContestUserRoleRepository? _contestUserRoles;
    public IContestUserRoleRepository ContestUserRoles => _contestUserRoles ??= new ContestUserRoleRepository(UowDbContext, _mapper);

    private IInvitationRepository? _invitations;

    public IInvitationRepository Invitations => _invitations ?? new InvitationRepository(UowDbContext, _mapper);
}