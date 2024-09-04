using App.BLL.DTO;
using App.BLL.DTO.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class AppUserIndexViewModel
{
    public IEnumerable<UserRoleModel> UserRoleModels { get; set; } = default!; 
    public Guid ContestId { get; set; }
}

public class UserRoleModel
{
    public UserContestPackage Package { get; set; } = default!;
    public ContestRole Role { get; set; } = default!;
}