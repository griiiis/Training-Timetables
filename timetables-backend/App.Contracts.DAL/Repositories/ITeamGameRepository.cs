using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITeamGameRepository : IEntityRepository<DALDTO.TeamGame>, ITeamGameRepositoryCustom<DALDTO.TeamGame>
{
}

public interface ITeamGameRepositoryCustom<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
}