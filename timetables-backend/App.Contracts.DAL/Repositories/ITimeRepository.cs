using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITimeRepository : IEntityRepository<DALDTO.Time>, ITimeRepositoryCustom<DALDTO.Time>
{
}

public interface ITimeRepositoryCustom<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
    
    Task<IEnumerable<TEntity>> GetAllCurrentContestAsync(Guid contestId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllCurrentContestWithTimesOfDayAsync(Guid contestId = default, bool noTracking = true);
}