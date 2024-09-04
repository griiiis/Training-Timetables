using App.BLL.DTO.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class AppUserEditViewModel
{
    public AppUser AppUser { get; set; } = default!;
    public SelectList? RoleSelectList { get; set; }
    public Guid SelectedRoleId { get; set; }
    public Guid ContestId { get; set; }
}