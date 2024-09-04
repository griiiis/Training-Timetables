using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class ContestUserRoleService : BaseEntityService<App.DAL.DTO.ContestUserRole, App.BLL.DTO.ContestUserRole, IContestUserRoleRepository, IAppUnitOfWork>, IContestUserRoleService
{
    public ContestUserRoleService(IAppUnitOfWork uow, IContestUserRoleRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.ContestUserRole, App.BLL.DTO.ContestUserRole>(mapper))
    {
    }
    
    public new async Task<IEnumerable<App.BLL.DTO.ContestUserRole>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId)).Select(de => Mapper.Map(de))!;
    }

    public async Task<ContestUserRole> GetContestUserRole(Guid userId, Guid contestId, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GetContestUserRole(userId, contestId))!;
    }

    public bool IfContestTrainer(Guid userId, Guid contestId)
    {
        return Repository.IfContestTrainer(userId, contestId);
    }
}