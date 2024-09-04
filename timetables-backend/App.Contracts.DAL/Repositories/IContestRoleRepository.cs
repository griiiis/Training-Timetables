using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IContestRoleRepository : IEntityRepository<DALDTO.ContestRole>, IContestRoleRepositoryCustom<DALDTO.ContestRole>
{
    
}

public interface IContestRoleRepositoryCustom<TEntity>
{
    Task<Guid> ContestRoleId(string roleName);
    Task<IEnumerable<TEntity>> ContestRoles(Guid contestId);
}