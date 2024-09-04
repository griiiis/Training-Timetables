using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TeamRepository : BaseEntityRepository<APPDomain.Team, DALDTO.Team, AppDbContext>, ITeamRepository
{
    private readonly UserManager<AppUser> _userManager;
    public TeamRepository(AppDbContext dbContext, IMapper mapper, UserManager<AppUser> userManager) : base(dbContext, new DalDomainMapper<APPDomain.Team,DALDTO.Team>(mapper))
    {
        _userManager = userManager;
    }
    
    public async Task<IEnumerable<DALDTO.Team>> GetAllCurrentContestAsync(Guid contestId)
    {
        var rolePreferencesAppUserIds = (await CreateQuery()
                .Include(e => e.GameType)
                .ThenInclude(t => t!.RolePreferences)
                .ToListAsync())
            .SelectMany(e => e.GameType!.RolePreferences.Where(e => e.ContestId.Equals(contestId)))
            .Select(e => e.AppUserId).ToList();
        
        
        return (await CreateQuery()
            .Include(t => t.UserContestPackages)
            .ThenInclude(t => t.AppUser)
            .Include(l => l.Level)
            .Include(g => g.GameType)
            .ToListAsync())
            .Where(e => e.UserContestPackages!
                .Any(c => c.ContestId.Equals(contestId)) && e.UserContestPackages
                .Any(t => !rolePreferencesAppUserIds.Contains(t.AppUserId)))
            .Select(de => Mapper.Map(de)).ToList();
        
    }

    
}