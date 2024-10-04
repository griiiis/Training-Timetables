using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UserContestPackageRepository :
    BaseEntityRepository<APPDomain.UserContestPackage, DALDTO.UserContestPackage, AppDbContext>,
    IUserContestPackageRepository
{
    private readonly UserManager<AppUser> _userManager;

    public UserContestPackageRepository(AppDbContext dbContext, IMapper mapper, UserManager<AppUser> userManager) :
        base(dbContext,
            new DalDomainMapper<APPDomain.UserContestPackage, DALDTO.UserContestPackage>(mapper))
    {
        _userManager = userManager;
    }

    public new async Task<IEnumerable<DALDTO.UserContestPackage>> GetAllAsync()
    {
        var rolePreferencesAppUserIds = await CreateQuery()
            .SelectMany(e => e.PackageGameTypeTime!.GameType!.RolePreferences
                .Select(e => e.AppUserId)).ToListAsync();
        
        return (await CreateQuery()
            .Include(u => u.PackageGameTypeTime)
            .ThenInclude(g => g!.GameType)
            .Include(l => l.Level)
            .Include(c => c.Contest)
            .Where(e => !rolePreferencesAppUserIds.Contains(e.AppUserId))
            
            .ToListAsync()).Select(de => Mapper.Map(de));
    }

    public async Task<DALDTO.UserContestPackage?> GetUserContestPackage(Guid contestId, Guid userId)
    {
        return Mapper.Map(await CreateQuery(userId)
            .Include(l => l.Level)
            .Include(u => u.AppUser)
            .Include(c => c.Contest)
            .Include(u => u.PackageGameTypeTime)
            .ThenInclude(g => g!.GameType)
            .Where(e => e.ContestId.Equals(contestId))
            .FirstOrDefaultAsync());
    }

    public new async Task<DALDTO.UserContestPackage?> FirstOrDefaultAsync(Guid id, Guid userId = default,
        bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, noTracking).Include(u => u.AppUser).Include(c => c.Contest)
            .Include(u => u.PackageGameTypeTime).ThenInclude(g => g!.GameType)
            .FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }

    //All users (trainers & players)
    public async Task<IEnumerable<DALDTO.UserContestPackage>> GetContestUsers(Guid contestId)
    {
        return (await CreateQuery()
            .Include(e => e.AppUser)
            .Include(e => e.Team)
            .Where(e => e.ContestId.Equals(contestId))
            .ToListAsync()).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<DALDTO.UserContestPackage>> GetCurrentUserPackages(Guid userId)
    {
        return (await CreateQuery()
            .Where(e => e.AppUserId.Equals(userId))
            .ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<DALDTO.UserContestPackage>> GetContestTeachers(Guid contestId)
    {
        var rolePreferencesAppUserIds = await CreateQuery().SelectMany(e => e.PackageGameTypeTime!.GameType!.RolePreferences.Where(e => e.ContestId.Equals(contestId)).Select(e => e.AppUserId)).ToListAsync();
        var packages = await CreateQuery().ToListAsync();
        
        
        return (await CreateQuery()
                .Include(t => t.Team!.TeamGames)
                .ThenInclude(e => e.Game)
                .Include(e => e.AppUser)
                .ThenInclude(e => e!.RolePreferences)
                .Where(e => e.ContestId.Equals(contestId) && rolePreferencesAppUserIds.Contains(e.AppUserId)).ToListAsync())
            .Select(de => Mapper.Map(de));
    }
    
    public async Task<IEnumerable<DALDTO.UserContestPackage>> GetContestParticipants(Guid contestId)
    {
        var rolePreferencesAppUserIds = await CreateQuery().SelectMany(e => e.PackageGameTypeTime!.GameType!.RolePreferences.Where(e => e.ContestId.Equals(contestId))).Select(e => e.AppUserId).ToListAsync();
        
        return (await CreateQuery()
            .Include(t => t.Team)
            .ThenInclude(g => g!.TeamGames)
            .ThenInclude(g => g.Game)
            .Include(e => e.AppUser)
            .Include(t => t.Team)
            .ThenInclude(l => l!.Level)
            .Include(t => t.Team)
            .ThenInclude(l => l!.GameType)
            .Where(e => e.ContestId == contestId && !rolePreferencesAppUserIds.Contains(e.AppUserId))
            .ToListAsync()).Select(de => Mapper.Map(de));
    }
    
    public async Task<IEnumerable<DALDTO.UserContestPackage>> GetContestTeammates(Guid contestId,
        Guid teamId)
    {
        var rolePreferencesAppUserIds = await CreateQuery().SelectMany(e => e.PackageGameTypeTime!.GameType!.RolePreferences.Where(e => e.ContestId.Equals(contestId))).Select(e => e.AppUserId).ToListAsync();
        
        return (await CreateQuery()
            .Include(e => e.AppUser)
            .Where(e => e.ContestId == contestId &&
                        e.TeamId == teamId && 
                        !rolePreferencesAppUserIds.Contains(e.AppUserId))
            .ToListAsync()).Select(de => Mapper.Map(de));
    }

    public bool AnyTeams(Guid contestId)
    {
        return !CreateQuery()
            .Where(e => e.ContestId.Equals(contestId))
            .Any(e => e.Contest!.UserContestPackages.Any());
    }
    
    public bool IfAlreadyJoined(Guid contestId, Guid userId)
    {
        return CreateQuery(userId)
            .Any(e => e.ContestId.Equals(contestId));
    }
}