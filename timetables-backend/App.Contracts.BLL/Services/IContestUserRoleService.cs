using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;

namespace App.Contracts.BLL.Services;

public interface IContestUserRoleService : IEntityService<App.BLL.DTO.ContestUserRole>, IContestUserRoleRepositoryCustom<App.BLL.DTO.ContestUserRole>
{
}