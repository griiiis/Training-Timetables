using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IContestRoleService : IEntityService<App.BLL.DTO.ContestRole>, IContestRoleRepositoryCustom<App.BLL.DTO.ContestRole>
{
}