using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class TeamService : BaseEntityService<App.DAL.DTO.Team, App.BLL.DTO.Team, ITeamRepository, IAppUnitOfWork>, ITeamService
{
    public TeamService(IAppUnitOfWork uow, ITeamRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Team, App.BLL.DTO.Team>(mapper))
    {
    }

    public async Task<IEnumerable<Team>> GetAllCurrentContestAsync(Guid contestId)
    {
        return (await Repository.GetAllCurrentContestAsync(contestId)).Select(de => Mapper.Map(de));
    }

}