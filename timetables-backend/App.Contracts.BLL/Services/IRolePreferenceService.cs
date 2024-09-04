using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IRolePreferenceService : IEntityService<App.BLL.DTO.RolePreference>, IRolePreferenceRepositoryCustom<App.BLL.DTO.RolePreference>
{ 
    App.BLL.DTO.RolePreference AddRolePreferenceWithUser(Guid userId, App.BLL.DTO.RolePreference rolePreference);
    
    bool IsRolePreferenceOwnedByUser (Guid userId, Guid rolePreferenceId);
    App.BLL.DTO.RolePreference UpdateRolePreferenceWithUser(Guid userId, App.BLL.DTO.RolePreference rolePreference);
}