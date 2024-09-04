using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Enums;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.DAL.DTO.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public int Age { get; set; }

    public EGender Gender { get; set; }
    
    public ICollection<DAL.DTO.UserContestPackage>? UserContestPackages { get; set; }   
    //public ICollection<AppRefreshToken> RefreshTokens { get; set; }
    public ICollection<DAL.DTO.ContestType>? ContestTypes { get; set; }
    public ICollection<DAL.DTO.Court>? Courts { get; set; }
    public ICollection<DAL.DTO.GameType>? GameTypes { get; set; }
    public ICollection<DAL.DTO.Level>? Levels { get; set; }
    public ICollection<DAL.DTO.Location>? Locations { get; set; } 
    public ICollection<DAL.DTO.PackageGameTypeTime>? PackageGameTypeTimes { get; set; }
    public ICollection<DAL.DTO.Time>? Times { get; set; }
    public ICollection<DAL.DTO.TimeOfDay>? TimeOfDays { get; set; }
    public ICollection<DAL.DTO.RolePreference>? RolePreferences { get; set; } 
    
}