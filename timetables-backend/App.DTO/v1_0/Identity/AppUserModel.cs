using App.Domain.Identity;

namespace App.DTO.v1_0.Identity;

public class AppUserModel
{
    public AppUser AppUser { get; set; } = default!;
    public List<AppRole>? RoleSelectList { get; set; }
    public Guid SelectedRoleId { get; set; }
    
}