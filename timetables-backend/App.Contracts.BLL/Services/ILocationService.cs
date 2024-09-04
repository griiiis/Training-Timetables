using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ILocationService : IEntityService<App.BLL.DTO.Location>
{ 
    App.BLL.DTO.Location AddLocationWithUser(Guid userId, App.BLL.DTO.Location location);
    bool IsLocationOwnedByUser (Guid userId, Guid locationId);
    App.BLL.DTO.Location UpdateLocationWithUser(Guid userId, App.BLL.DTO.Location location);
}