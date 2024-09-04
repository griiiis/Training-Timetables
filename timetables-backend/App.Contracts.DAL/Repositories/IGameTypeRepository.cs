using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IGameTypeRepository : IEntityRepository<DALDTO.GameType>, IGameTypeRepositoryCustom<DALDTO.GameType>
{
    
}

public interface IGameTypeRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCurrentContestAsync(Guid contestId);
}