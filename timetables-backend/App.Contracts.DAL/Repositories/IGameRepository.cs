using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IGameRepository : IEntityRepository<DALDTO.Game>, IGameRepositoryCustom<DALDTO.Game>
{
}

public interface IGameRepositoryCustom<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetContestGamesWithoutTeachers(Guid contestId);
    Task<IEnumerable<TEntity>> GetContestGames(Guid contestId);
    bool AnyContestGames(Guid contestId);
    Task<IEnumerable<TEntity>> GetUserContestGames(Guid contestId, Guid userId);
}