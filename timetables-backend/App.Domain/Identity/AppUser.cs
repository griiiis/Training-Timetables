using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    [MaxLength(128)] [MinLength(1)] public string FirstName { get; set; } = default!;
    [MaxLength(128)] [MinLength(1)] public string LastName { get; set; } = default!;

    public int Age { get; set; }

    public EGender Gender { get; set; }
    
    public ICollection<UserContestPackage>? UserContestPackages { get; set; }   
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
    public ICollection<ContestType>? ContestTypes { get; set; }
    public ICollection<Court>? Courts { get; set; }
    public ICollection<GameType>? GameTypes { get; set; }
    public ICollection<Level>? Levels { get; set; }
    public ICollection<Location>? Locations { get; set; } 
    public ICollection<PackageGameTypeTime>? PackageGameTypeTimes { get; set; }
    public ICollection<Time>? Times { get; set; }
    public ICollection<TimeOfDay>? TimeOfDays { get; set; }
    public ICollection<RolePreference>? RolePreferences { get; set; }
    public ICollection<ContestUserRole>? ContestUserRoles { get; set; }
    
}