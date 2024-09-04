using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class TeamGameService : BaseEntityService<App.DAL.DTO.TeamGame, App.BLL.DTO.TeamGame, ITeamGameRepository, IAppUnitOfWork>, ITeamGameService
{
    public TeamGameService(IAppUnitOfWork uow, ITeamGameRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.TeamGame, App.BLL.DTO.TeamGame>(mapper))
    {
    }
}