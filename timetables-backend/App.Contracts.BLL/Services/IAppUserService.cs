using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IAppUserService : IEntityService<App.BLL.DTO.Identity.AppUser>, IAppUserRepositoryCustom<App.BLL.DTO.Identity.AppUser>
{
}