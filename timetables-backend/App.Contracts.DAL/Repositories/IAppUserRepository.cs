using Base.Contracts.DAL;
using DALDTO = App.DAL.DTO;

namespace App.Contracts.DAL.Repositories;

public interface IAppUserRepository : IEntityRepository<DALDTO.Identity.AppUser>,
    IAppUserRepositoryCustom<DALDTO.Identity.AppUser>
{

}

public interface IAppUserRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllContestUsers(Guid contestId);

    }

