using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITeamGameService : IEntityService<App.BLL.DTO.TeamGame>, ITeamGameRepositoryCustom<App.BLL.DTO.TeamGame>
{ 
    
}