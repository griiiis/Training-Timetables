using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IRolePreferenceRepository : IEntityRepository<DALDTO.RolePreference>, IRolePreferenceRepositoryCustom<DALDTO.RolePreference>
{ 
}

public interface IRolePreferenceRepositoryCustom<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
}