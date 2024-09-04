using App.Contracts.DAL.Repositories;
using App.DAL.DTO.Identity;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AppuserRepository : BaseEntityRepository<APPDomain.Identity.AppUser, DALDTO.Identity.AppUser, AppDbContext>, IAppUserRepository
{
    public AppuserRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.Identity.AppUser,DALDTO.Identity.AppUser>(mapper))
    {
    }


    public async Task<IEnumerable<AppUser>> GetAllContestUsers(Guid contestId)
    {
        return (await CreateQuery()
            .Where(e => e.UserContestPackages
                .Any(e => e.ContestId.Equals(contestId)))
            .ToListAsync()).Select(de => Mapper.Map(de));
    }
}