using App.Contracts.DAL.Repositories;
using App.DAL.DTO.Models;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContestRepository : BaseEntityRepository<APPDomain.Contest, DALDTO.Contest, AppDbContext>, IContestRepository
{
    
    
    public ContestRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.Contest,DALDTO.Contest>(mapper))
    {
    }
    public async Task<DALDTO.Contest?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {

        return Mapper.Map(await CreateQuery(userId, noTracking)
            .Include(c => c.ContestType)
            .Include(l => l.Location)
            .Include(l => l.ContestLevels)
            .ThenInclude(l => l.Level)
            .Include(l => l.ContestPackages)
            .ThenInclude(p => p.PackageGameTypeTime)
            .Include(l => l.ContestTimes)
            .ThenInclude(l => l.Time)
            .FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }

    public async Task<IEnumerable<DALDTO.Contest>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId)
            .Include(g => g.ContestLevels)
            .Include(g => g.ContestTimes)
            .Include(g => g.ContestPackages)
            .Include(c => c.ContestType)
            .Include(l => l.Location)
            .Include(l => l.ContestLevels)
            .Include(g => g.ContestGameTypes)
            .ThenInclude(g => g.GameType)
            .ToListAsync()).Select(de => Mapper.Map(de)!);
    }

    public async Task<IEnumerable<DALDTO.Contest>> GetUserContests(Guid userId)
    {
        return (await CreateQuery()
                .Include(e => e.UserContestPackages)
            .ToListAsync()).Where(e => e.UserContestPackages.
                Any(a => a.AppUserId == userId))
            .Select(de => Mapper.Map(de));
    }
    
}