using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class UserContestPackage : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.UserContestPackage), Name = nameof(PackageGameTypeTime))]
    public Guid PackageGameTypeTimeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.UserContestPackage), Name = nameof(PackageGameTypeTime))]
    public PackageGameTypeTime? PackageGameTypeTime { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.UserContestPackage), Name = nameof(HoursAvailable))]
    public decimal HoursAvailable { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Domain.UserContestPackage), Name = nameof(AppUser))]
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.UserContestPackage), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
}