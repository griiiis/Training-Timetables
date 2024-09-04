using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IContestRepository : IEntityRepository<DALDTO.Contest>, IContestRepositoryCustom<DALDTO.Contest>
{ 
}

public interface IContestRepositoryCustom<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetUserContests(Guid userId);
}