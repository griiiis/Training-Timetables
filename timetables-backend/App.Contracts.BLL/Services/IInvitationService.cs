using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IInvitationService : IEntityService<App.BLL.DTO.Invitation>, IInvitationRepositoryCustom<App.BLL.DTO.Invitation>
{ 
}