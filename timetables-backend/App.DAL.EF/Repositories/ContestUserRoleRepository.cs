using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContestUserRoleRepository : BaseEntityRepository<APPDomain.ContestUserRole, DALDTO.ContestUserRole, AppDbContext>, IContestUserRoleRepository
{
    public ContestUserRoleRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.ContestUserRole,DALDTO.ContestUserRole>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.ContestUserRole>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId)
            .Include(c => c.AppUser)
            .Include(c => c.ContestRole)
            .ToListAsync()).Select(de => Mapper.Map(de)!);
    }

    public async Task<DALDTO.ContestUserRole> GetContestUserRole(Guid userId, Guid contestId, bool noTracking = true)
    {
        return (await CreateQuery()
                .Where(e => e.AppUserId.Equals(userId) && e.ContestRole!.ContestId.Equals(contestId))
                .Include(e => e.ContestRole)
            .ToListAsync())
            .Select(e => Mapper.Map(e)).FirstOrDefault()!;
    }

    public bool IfContestTrainer(Guid userId, Guid contestId)
    {
        return CreateQuery().Any(e => e.AppUserId.Equals(userId) && e.ContestRole!.ContestId.Equals(contestId) && e.ContestRole.ContestRoleName.Equals("Trainer"));
    }
}