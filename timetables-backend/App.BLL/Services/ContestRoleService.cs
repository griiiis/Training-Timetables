using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO.Identity;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using ContestRole = App.DAL.DTO.ContestRole;

namespace App.BLL.Services;

public class ContestRoleService : BaseEntityService<App.DAL.DTO.ContestRole, App.BLL.DTO.ContestRole, IContestRoleRepository, IAppUnitOfWork>, IContestRoleService
{
    public ContestRoleService(IAppUnitOfWork uow, IContestRoleRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.ContestRole, App.BLL.DTO.ContestRole>(mapper))
    {
    }

    public async Task<Guid> ContestRoleId(string roleName)
    {
        return await Repository.ContestRoleId(roleName);
    }

    public async Task<IEnumerable<App.BLL.DTO.ContestRole>> ContestRoles(Guid contestId)
    {
        return (await Repository.ContestRoles(contestId)).Select(e => Mapper.Map(e));
    }
}