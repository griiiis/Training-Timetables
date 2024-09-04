using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO.Identity;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class AppUserService : BaseEntityService<App.DAL.DTO.Identity.AppUser, App.BLL.DTO.Identity.AppUser, IAppUserRepository, IAppUnitOfWork>, IAppUserService
{
    public AppUserService(IAppUnitOfWork uow, IAppUserRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Identity.AppUser, App.BLL.DTO.Identity.AppUser>(mapper))
    {
    }

    public async Task<IEnumerable<App.BLL.DTO.Identity.AppUser>> GetAllContestUsers(Guid contestId)
    {
        return (await Repository.GetAllContestUsers(contestId)).Select(e => Mapper.Map(e))!;
    }
}