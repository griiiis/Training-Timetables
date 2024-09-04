using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CourtRepository : BaseEntityRepository<APPDomain.Court, DALDTO.Court, AppDbContext>, ICourtRepository
{
    public CourtRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.Court,DALDTO.Court>(mapper))
    {
    }
    
    public async Task<IEnumerable<DALDTO.Court>> GetAllCurrentContestAsync(Guid contestId, bool noTracking = true)
    {
        return (await CreateQuery(default, noTracking)
                .Include(l => l.Location)
                .ThenInclude(c => c!.Contests)
                .ToListAsync())
            .Where(e => e.Location!.Contests.Any(l => l.Id.Equals(contestId)))
            .Select(de => Mapper.Map(de));
    }
    
    public async Task<IEnumerable<DALDTO.Court>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking)
                .Include(l => l.Location)
                .Include(g => g.GameType)
                .ToListAsync())
            .Select(de => Mapper.Map(de));
    }
    
    
}