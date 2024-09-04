using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class TimeTeamService : BaseEntityService<App.DAL.DTO.TimeTeam, App.BLL.DTO.TimeTeam, ITimeTeamRepository, IAppUnitOfWork>,
    ITimeTeamService
{
    public TimeTeamService(IAppUnitOfWork uow, ITimeTeamRepository repository, IMapper mapper)
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.TimeTeam, App.BLL.DTO.TimeTeam>(mapper))
    {
    }

    public async Task<int> RemoveTeamTimeTeamsAsync(Guid teamId)
    {
        return await Repository.RemoveTeamTimeTeamsAsync(teamId);
    }

    public async Task<IEnumerable<App.BLL.DTO.TimeTeam>> GetContestTeamTimes(Guid teamId)
    {
        return (await Repository.GetContestTeamTimes(teamId)).Select(e => Mapper.Map(e));
    }
}