using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IPackageGameTypeTimeService : IEntityService<App.BLL.DTO.PackageGameTypeTime>, IPackageGameTypeTimeRepositoryCustom<App.BLL.DTO.PackageGameTypeTime>
{ 
    App.BLL.DTO.PackageGameTypeTime AddPackageGameTypeTimeWithUser(Guid userId, App.BLL.DTO.PackageGameTypeTime packageGameTypeTime);
    bool IsPackageGameTypeTimeOwnedByUser (Guid userId, Guid packageGameTypeTimeId);
    App.BLL.DTO.PackageGameTypeTime UpdatePackageGameTypeTimeWithUser(Guid userId, App.BLL.DTO.PackageGameTypeTime packageGameTypeTime);
    
}