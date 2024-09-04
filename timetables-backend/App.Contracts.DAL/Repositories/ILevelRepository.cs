using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ILevelRepository : IEntityRepository<DALDTO.Level>, ILevelRepositoryCustom<DALDTO.Level>
{
    
}

public interface ILevelRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCurrentContestAsync(Guid contestId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
}