using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace App.DAL.EF.Repositories;

public class ContestRoleRepository : BaseEntityRepository<APPDomain.ContestRole, DALDTO.ContestRole, AppDbContext>, IContestRoleRepository
{
    public ContestRoleRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.ContestRole,DALDTO.ContestRole>(mapper))
    {
    }
    
    public async Task<IEnumerable<DALDTO.ContestRole>> ContestRoles(Guid contestId)
    {
        return (await CreateQuery(default).Where(e => e.ContestId.Equals(contestId)).ToListAsync()).Select(de => Mapper.Map(de)!);
    }

    public async Task<Guid> ContestRoleId(string roleName)
    {
        return await CreateQuery(default).Where(e => e.ContestRoleName.Equals(roleName)).Select(e => e.Id).FirstOrDefaultAsync();
    }
}