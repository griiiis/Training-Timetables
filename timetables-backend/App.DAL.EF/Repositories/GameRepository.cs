using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class GameRepository : BaseEntityRepository<APPDomain.Game, DALDTO.Game, AppDbContext>, IGameRepository
{
    public GameRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext,
        new DalDomainMapper<APPDomain.Game, DALDTO.Game>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.Game>> GetContestGamesWithoutTeachers(Guid contestId)
    {
        return (await CreateQuery()
            .Include(t=> t.TeamGames)
            .ThenInclude(t=> t.Team)
            .ThenInclude(u => u!.UserContestPackages)
            .ThenInclude(t => t.AppUser)
            .Include(g => g.Level)
            .Include(g => g.Court)
            .Include(g => g.GameType)
            .OrderBy(g => g.GameType)
            .Where(e => e.ContestId.Equals(contestId))
            .OrderBy(e => e.From).ToListAsync()).Select(
            de => Mapper.Map(de));
    }

    public async Task<IEnumerable<DALDTO.Game>> GetContestGames(Guid contestId)
    {
        return (await CreateQuery()
            .Include(g => g.Level)
            .Include(g => g.Court)
            .Include(g => g.GameType)
            .OrderBy(g => g.GameType)
            .Where(e => e.ContestId.Equals(contestId))
            .OrderBy(e => e.From).ToListAsync()).Select(
            de => Mapper.Map(de));
    }

    public bool AnyContestGames(Guid contestId)
    {
        return CreateQuery()
            .Any(e => e.ContestId.Equals(contestId));
    }

    public async Task<IEnumerable<DALDTO.Game>> GetUserContestGames(Guid contestId, Guid userId)
    {
        return (await CreateQuery()
                .Include(u => u.TeamGames)
                .ThenInclude(u => u.Team)
                .ThenInclude(u => u!.UserContestPackages)
                .ThenInclude(t => t.AppUser)
                .Include(g => g.Contest)
                .Include(g => g.Court)
                .Include(g => g.GameType)
                .Include(g => g.Level)
                .OrderBy(g => g.GameType)
                .Where(g => g.TeamGames.Any(t => t.Team!.UserContestPackages.Any(a => a.AppUserId.Equals(userId)))
                            && g.ContestId.Equals(contestId))
                .ToListAsync())
            .Select(de => Mapper.Map(de));
    }

    public new async Task<DALDTO.Game?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, noTracking)
            .Include(g => g.Contest)
            .FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }
}