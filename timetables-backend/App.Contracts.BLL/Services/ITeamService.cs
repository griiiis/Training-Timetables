using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITeamService : IEntityService<App.BLL.DTO.Team>, ITeamRepositoryCustom<App.BLL.DTO.Team>
{ 
    
}