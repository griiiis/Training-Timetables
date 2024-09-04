using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TimeTeamRepository : BaseEntityRepository<APPDomain.TimeTeam, DALDTO.TimeTeam, AppDbContext>, ITimeTeamRepository
{
    public TimeTeamRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.TimeTeam, DALDTO.TimeTeam>(mapper))
    {
    }

    public async Task<int> RemoveTeamTimeTeamsAsync(Guid teamId)
    {
        return await CreateQuery()
            .Where(e => e.TeamId.Equals(teamId))
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<DALDTO.TimeTeam>> GetContestTeamTimes(Guid teamId)
    {
        return (await CreateQuery().Where(e => e.TeamId.Equals(teamId)).ToListAsync()).Select(e => Mapper.Map(e));
    }
}