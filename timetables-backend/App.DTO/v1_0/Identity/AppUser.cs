using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.DTO.v1_0.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{ 
    //To show players in games
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public ICollection<RolePreference>? RolePreferences { get; set; }
}