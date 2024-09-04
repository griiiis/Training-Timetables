using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IContestTypeService : IEntityService<App.BLL.DTO.ContestType>
{ 
    App.BLL.DTO.ContestType AddContestTypeWithUser(Guid userId, App.BLL.DTO.ContestType contestType);
    bool IsContestTypeOwnedByUser (Guid userId, Guid contestTypeId);
    App.BLL.DTO.ContestType UpdateContestTypeWithUser(Guid userId, App.BLL.DTO.ContestType contestType);
    
}