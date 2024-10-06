using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class InvitationService : BaseEntityService<App.DAL.DTO.Invitation, App.BLL.DTO.Invitation, IInvitationRepository, IAppUnitOfWork>, IInvitationService
{
    public InvitationService(IAppUnitOfWork uow, IInvitationRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Invitation, App.BLL.DTO.Invitation>(mapper))
    {
    }

    public async Task<Invitation?> FirstOrDefaultAsync(Guid id)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(id));
    }

    public async Task<IEnumerable<Invitation>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).Select(e => Mapper.Map(e))!;
    }
}