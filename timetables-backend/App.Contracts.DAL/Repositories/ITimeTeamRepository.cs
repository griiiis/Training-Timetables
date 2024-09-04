using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITimeTeamRepository : IEntityRepository<DALDTO.TimeTeam>, ITimeTeamRepositoryCustom<DALDTO.TimeTeam>
{
}

public interface ITimeTeamRepositoryCustom<TEntity>
{
    Task<int> RemoveTeamTimeTeamsAsync(Guid teamId);
    Task<IEnumerable<TEntity>> GetContestTeamTimes(Guid teamId);
}