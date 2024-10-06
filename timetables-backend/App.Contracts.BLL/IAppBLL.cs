using App.BLL.DTO;
using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IContestService Contests { get; }
    IContestTypeService ContestTypes { get; }
    IEntityService<ContestLevel> ContestLevels { get; }
    IEntityService<ContestPackage> ContestPackages { get; }
    IEntityService<ContestTime> ContestTimes { get; }
    IEntityService<ContestGameType> ContestGameTypes { get; }
    ILocationService Locations { get; }
    ICourtService Courts { get; }
    IGameService Games { get; }
    IGameTypeService GameTypes { get; }
    ILevelService Levels { get; }
    IPackageGameTypeTimeService PackageGameTypeTimes { get; }
    IRolePreferenceService RolePreferences { get; }
    ITeamGameService TeamGames { get; }
    ITimeTeamService TimeTeams { get;  }
    ITimeOfDayService TimeOfDays { get; }
    ITeamService Teams { get; }
    IUserContestPackageService UserContestPackages { get; }
    ITimeService Times { get; }
    IAppUserService AppUsers { get; }
    IContestRoleService ContestRoles { get; }
    IContestUserRoleService ContestUserRoles { get; }
    IInvitationService Invitations { get; }
}