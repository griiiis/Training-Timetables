using App.BLL.DTO;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF;
using App.Domain.Identity;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;

namespace App.BLL;

public class AppBll: BaseBLL<AppDbContext> ,IAppBLL

{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    private readonly UserManager<AppUser> _userManager;
    
    public AppBll(IAppUnitOfWork uoW, IMapper mapper, UserManager<AppUser> userManager) : base(uoW)
    {
        _mapper = mapper;
        _userManager = userManager;
        _uow = uoW;
    }

    private IContestService? _contests;
    public IContestService Contests => _contests ??= new ContestService(_uow, _uow.Contests, _mapper);

    private IContestTypeService? _contestTypes;
    public IContestTypeService ContestTypes => _contestTypes ??= new ContestTypeService(_uow, _uow.ContestTypes, _mapper);
    
    private IEntityService<ContestLevel>? _contestLevel;
    public IEntityService<ContestLevel> ContestLevels =>
        _contestLevel ??= new BaseEntityService<DAL.DTO.ContestLevel, BLL.DTO.ContestLevel, IEntityRepository<DAL.DTO.ContestLevel>, IAppUnitOfWork>(_uow,_uow.ContestLevels, new BLLDalMapper<DAL.DTO.ContestLevel, App.BLL.DTO.ContestLevel>(_mapper));

    private IEntityService<ContestGameType>? _contestGameTypes;
    public IEntityService<ContestGameType> ContestGameTypes =>
        _contestGameTypes ??= new BaseEntityService<DAL.DTO.ContestGameType, BLL.DTO.ContestGameType, IEntityRepository<DAL.DTO.ContestGameType>, IAppUnitOfWork>(_uow,_uow.ContestGameTypes, new BLLDalMapper<DAL.DTO.ContestGameType, App.BLL.DTO.ContestGameType>(_mapper));

    private IEntityService<ContestPackage>? _contestPackage;
    public IEntityService<ContestPackage> ContestPackages =>
        _contestPackage ??= new BaseEntityService<DAL.DTO.ContestPackage, BLL.DTO.ContestPackage, IEntityRepository<DAL.DTO.ContestPackage>, IAppUnitOfWork>(_uow,_uow.ContestPackages, new BLLDalMapper<DAL.DTO.ContestPackage, App.BLL.DTO.ContestPackage>(_mapper));

    private IEntityService<ContestTime>? _contestTime;
    public IEntityService<ContestTime> ContestTimes =>
        _contestTime ??= new BaseEntityService<DAL.DTO.ContestTime, BLL.DTO.ContestTime, IEntityRepository<DAL.DTO.ContestTime>, IAppUnitOfWork>(_uow, _uow.ContestTimes, new BLLDalMapper<DAL.DTO.ContestTime, App.BLL.DTO.ContestTime>(_mapper));


    private ILocationService? _locations;
    public ILocationService Locations => _locations ??= new LocationService(_uow, _uow.Locations, _mapper);

    private ICourtService? _courts;
    public ICourtService Courts => _courts ??= new CourtService(_uow, _uow.Courts, _mapper);

    private IGameService? _games;
    public IGameService Games => _games ??= new GameService(_uow, _uow.Games, _mapper);

    private IGameTypeService? _gameTypes;
    public IGameTypeService GameTypes => _gameTypes ??= new GameTypeService(_uow, _uow.GameTypes, _mapper);

    private ILevelService? _levels;
    public ILevelService Levels => _levels ??= new LevelService(_uow, _uow.Levels, _mapper);

    private IPackageGameTypeTimeService? _packageGameTypeTimes;
    public IPackageGameTypeTimeService PackageGameTypeTimes => _packageGameTypeTimes ??= new PackageGameTypeTimeService(_uow, _uow.PackageGameTypeTimes, _mapper);

    private IRolePreferenceService? _rolePreferences;
    public IRolePreferenceService RolePreferences => _rolePreferences ??= new RolePreferenceService(_uow, _uow.RolePreferences, _mapper, _userManager);

    private ITeamGameService? _teamGames;
    public ITeamGameService TeamGames => _teamGames ??= new TeamGameService(_uow,_uow.TeamGames, _mapper);

    private ITeamService? _teams;
    public ITeamService Teams => _teams ??= new TeamService(_uow,_uow.Teams, _mapper);

    private ITimeOfDayService? _timeOfDays;
    public ITimeOfDayService TimeOfDays => _timeOfDays ??= new TimeOfDayService(_uow,_uow.TimeOfDays, _mapper);
    
    private ITimeTeamService? _timeTeams;
    public ITimeTeamService TimeTeams => _timeTeams ??= new TimeTeamService(_uow,_uow.TimeTeams, _mapper);

    private ITimeService? _times;
    public ITimeService Times => _times ??= new TimeService(_uow,_uow.Times, _mapper);

    private IAppUserService? _appUsers; 
    public IAppUserService AppUsers => _appUsers ??= new AppUserService(_uow,_uow.AppUsers, _mapper);

    private IUserContestPackageService? _userContestPackages;
    public IUserContestPackageService UserContestPackages => _userContestPackages ??= new UserContestPackageService(_uow,_uow.UserContestPackages, _mapper);
    
    private IContestRoleService? _contestRoles;
    public IContestRoleService ContestRoles => _contestRoles ??= new ContestRoleService(_uow,_uow.ContestRoles, _mapper);
    
    private IContestUserRoleService? _contestUserRoles;
    public IContestUserRoleService ContestUserRoles => _contestUserRoles ??= new ContestUserRoleService(_uow,_uow.ContestUserRoles, _mapper);
    
    private IInvitationService? _invitations;
    public IInvitationService Invitations => _invitations ?? new InvitationService(_uow,_uow.Invitations, _mapper);
}