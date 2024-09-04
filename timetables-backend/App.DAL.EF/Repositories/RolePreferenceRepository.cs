using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RolePreferenceRepository : BaseEntityRepository<APPDomain.RolePreference, DALDTO.RolePreference, AppDbContext>, IRolePreferenceRepository
{
    public RolePreferenceRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.RolePreference, DALDTO.RolePreference>(mapper))
    {
    }
    public new async Task<IEnumerable<DALDTO.RolePreference>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking)
            .Include(r => r.GameType)
            .Include(r => r.Level)
            .ToListAsync())
            .Select(de => Mapper.Map(de));
    }
    
    public new async Task<DALDTO.RolePreference?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, noTracking)
            .Include(r => r.GameType)
            .Include(r => r.Level)
            .FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }
}