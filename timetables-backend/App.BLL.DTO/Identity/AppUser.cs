using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Enums;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.BLL.DTO.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{ 
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
    public EGender Gender { get; set; }
    public ICollection<BLL.DTO.RolePreference>? RolePreferences { get; set; } 
}