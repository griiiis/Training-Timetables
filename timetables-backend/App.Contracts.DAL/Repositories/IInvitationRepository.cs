using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IInvitationRepository : IEntityRepository<DALDTO.Invitation>, IInvitationRepositoryCustom<DALDTO.Invitation>
{
}

public interface IInvitationRepositoryCustom<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();
}