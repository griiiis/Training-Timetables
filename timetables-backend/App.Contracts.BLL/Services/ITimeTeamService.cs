using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITimeTeamService : IEntityService<App.BLL.DTO.TimeTeam>, ITimeTeamRepositoryCustom<App.BLL.DTO.TimeTeam>
{ 
    
}