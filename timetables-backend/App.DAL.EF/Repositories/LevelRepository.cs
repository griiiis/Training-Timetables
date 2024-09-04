using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class LevelRepository : BaseEntityRepository<APPDomain.Level, DALDTO.Level, AppDbContext>, ILevelRepository
{
    public LevelRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.Level,DALDTO.Level>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.Level>> GetAllCurrentContestAsync(Guid contestId, bool noTracking = true)
    {
        return (await CreateQuery()
            .Where(e => e.ContestLevels
                .Any(l => l.ContestId.Equals(contestId)))
            .OrderBy(e => e.Title)
            .ToListAsync())
            .Select(de => Mapper.Map(de));
    }
    
    public new async Task<IEnumerable<DALDTO.Level>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking)
            .OrderBy(e => e.Title)
            .ToListAsync()).
            Select(de => Mapper.Map(de));
    }
}