using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;

namespace App.Contracts.DAL.Repositories;

public interface IContestUserRoleRepository : IEntityRepository<DALDTO.ContestUserRole>, IContestUserRoleRepositoryCustom<DALDTO.ContestUserRole>
{
    
}

public interface IContestUserRoleRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
    Task<TEntity> GetContestUserRole(Guid userId, Guid contestId, bool noTracking = true);
    bool IfContestTrainer(Guid userId, Guid contestId);
}