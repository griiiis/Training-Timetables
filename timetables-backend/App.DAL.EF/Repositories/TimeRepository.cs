using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TimeRepository : BaseEntityRepository<APPDomain.Time, DALDTO.Time, AppDbContext>, ITimeRepository
{
    public TimeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.Time,DALDTO.Time>(mapper))
    {
    }
    
    public new async Task<DALDTO.Time?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {

        return Mapper.Map(await CreateQuery(userId, noTracking)
            .Include(c => c.TimeOfDay)
            .FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }
    
    public new async Task<IEnumerable<DALDTO.Time>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking)
            .Include(t => t.TimeOfDay)
            .OrderBy(e => e.From)
            .ToListAsync())
            .Select(de => Mapper.Map(de));
    }
    
    public async Task<IEnumerable<DALDTO.Time>> GetAllCurrentContestAsync(Guid contestId, bool noTracking = true)
    {
        return (await CreateQuery()
            .Where(e => e.ContestTimes
                .Any(l => l.ContestId.Equals(contestId)))
            .ToListAsync())
            .OrderBy(e => e.From)
            .Select(de => Mapper.Map(de));
    }
    
    public async Task<IEnumerable<DALDTO.Time>> GetAllCurrentContestWithTimesOfDayAsync(Guid contestId, bool noTracking = true)
    {
        return (await CreateQuery()
            .Include(t => t.TimeOfDay)
            .Where(e => e.ContestTimes
                .Any(l => l.ContestId.Equals(contestId)))
            .ToListAsync())
            .OrderBy(e => e.From)
            .Select(de => Mapper.Map(de));
    }

}