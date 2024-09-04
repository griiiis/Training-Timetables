using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PackageGameTypeTimeRepository : BaseEntityRepository<APPDomain.PackageGameTypeTime, DALDTO.PackageGameTypeTime, AppDbContext>, IPackageGameTypeTimeRepository
{
    public PackageGameTypeTimeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.PackageGameTypeTime,DALDTO.PackageGameTypeTime>(mapper))
    {
    }
    
    public new async Task<IEnumerable<DALDTO.PackageGameTypeTime>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking)
            .Include(p => p.GameType)
            .ToListAsync())
            .Select(de => Mapper.Map(de));
    }

    public async Task<IEnumerable<DALDTO.PackageGameTypeTime>> GetAllCurrentContestAsync(Guid contestId)
    {
        return (await CreateQuery()
            .Where(e => e.ContestPackages
                .Any(e => e.ContestId.Equals(contestId)))
            .OrderBy(e => e.PackageGtName)
            .ToListAsync()).Select(de => Mapper.Map(de));
    }

    public new async Task<DALDTO.PackageGameTypeTime?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, noTracking)
            .Include(p => p.GameType)
            .FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }

}