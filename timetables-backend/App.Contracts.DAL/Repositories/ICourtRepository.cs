using App.Domain;
using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ICourtRepository : IEntityRepository<DALDTO.Court>, ICourtRepositoryCustom<DALDTO.Court>
{
}

public interface ICourtRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllCurrentContestAsync(Guid contestId = default, bool noTracking = true);

}