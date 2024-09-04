using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TeamGameRepository : BaseEntityRepository<APPDomain.TeamGame,  DALDTO.TeamGame, AppDbContext>, ITeamGameRepository
{
    public TeamGameRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.TeamGame, DALDTO.TeamGame>(mapper))
    {
    }

    public new async Task<IEnumerable<DALDTO.TeamGame>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking).Include(t => t.Game).Include(t => t.Team).ToListAsync()).Select(de => Mapper.Map(de));
    }

    public new async Task<DALDTO.TeamGame?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, noTracking).Include(t => t.Game).Include(t => t.Team).FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }
}