using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class InvitationRepository : BaseEntityRepository<APPDomain.Invitation, DALDTO.Invitation, AppDbContext>, IInvitationRepository
{
    public InvitationRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.Invitation,DALDTO.Invitation>(mapper))
    {
    }
    
    public new async Task<DALDTO.Invitation?> FirstOrDefaultAsync(Guid id)
    {

        return Mapper.Map(await CreateQuery()
            .FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }
    
    public new async Task<IEnumerable<DALDTO.Invitation>> GetAllAsync()
    {
        return (await CreateQuery()
            .ToListAsync())
            .Select(de => Mapper.Map(de));
    }
}