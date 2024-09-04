using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IUserContestPackageService : IEntityService<App.BLL.DTO.UserContestPackage>, IUserContestPackageRepositoryCustom<App.BLL.DTO.UserContestPackage>
{ 
    App.BLL.DTO.UserContestPackage AddPackageWithUser(Guid userId, App.BLL.DTO.UserContestPackage userContestPackage);
    
    bool IsPackageOwnedByUser (Guid userId, Guid userContestPackageId);
    App.BLL.DTO.UserContestPackage UpdatePackageWithUser(Guid userId, App.BLL.DTO.UserContestPackage userContestPackage);
    
}