using App.Contracts.DAL.Repositories;
using App.Domain;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IContestRepository Contests { get; }
    IContestTypeRepository ContestTypes { get; }
    IEntityRepository<App.DAL.DTO.ContestGameType> ContestGameTypes { get; }
    IEntityRepository<App.DAL.DTO.ContestLevel> ContestLevels { get; }
    IEntityRepository<App.DAL.DTO.ContestTime> ContestTimes { get; }
    IEntityRepository<App.DAL.DTO.ContestPackage> ContestPackages { get; }
    ILocationRepository Locations { get; }
    ICourtRepository Courts { get; }
    IGameRepository Games { get; }
    IGameTypeRepository GameTypes { get; }
    ILevelRepository Levels { get; }
    IPackageGameTypeTimeRepository PackageGameTypeTimes { get; }
    IRolePreferenceRepository RolePreferences { get; }
    ITeamGameRepository TeamGames { get; }
    ITeamRepository Teams { get; }
    ITimeOfDayRepository TimeOfDays { get; }
    ITimeTeamRepository TimeTeams { get; }
    IUserContestPackageRepository UserContestPackages { get; }
    ITimeRepository Times { get; }
    IAppUserRepository AppUsers { get; }
    IContestRoleRepository ContestRoles { get; }
    IContestUserRoleRepository ContestUserRoles { get; }
}