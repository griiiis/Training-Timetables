using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using AutoMapper;
using Base.BLL;
using Microsoft.AspNetCore.Identity;

namespace App.BLL.Services;

public class RolePreferenceService : BaseEntityService<App.DAL.DTO.RolePreference, RolePreference, IRolePreferenceRepository, IAppUnitOfWork>, IRolePreferenceService
{
    public RolePreferenceService(IAppUnitOfWork uow, IRolePreferenceRepository repository, IMapper mapper, UserManager<AppUser> userManager) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.RolePreference, RolePreference>(mapper))
    {
    }
    
    public RolePreference AddRolePreferenceWithUser(Guid userId, RolePreference rolePreference)
    {
        var dto = Mapper.Map(rolePreference)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsRolePreferenceOwnedByUser(Guid userId, Guid rolePreferenceId)
    {
        return Repository.FirstOrDefault(rolePreferenceId)?.AppUserId == userId;
    }

    public RolePreference UpdateRolePreferenceWithUser(Guid userId, RolePreference rolePreference)
    {
        var dto = Mapper.Map(rolePreference)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}