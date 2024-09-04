using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IPackageGameTypeTimeRepository : IEntityRepository<DALDTO.PackageGameTypeTime>, IPackageGameTypeTimeRepositoryCustom<DALDTO.PackageGameTypeTime>
{
}

public interface IPackageGameTypeTimeRepositoryCustom<TEntity>
    {
        Task<TEntity?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true);

        Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
        Task<IEnumerable<TEntity>> GetAllCurrentContestAsync(Guid contestId);
    }