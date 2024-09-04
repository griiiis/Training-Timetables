using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITeamRepository : IEntityRepository<DALDTO.Team>, ITeamRepositoryCustom<DALDTO.Team>
{
}

public interface ITeamRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCurrentContestAsync(Guid contestId);
}