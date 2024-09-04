using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class  Contest : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(ContestName))]
    [Column(TypeName = "jsonb")]
    public LangStr ContestName { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(Description))]
    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(From))]
    public DateTime From { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(Until))]
    public DateTime Until { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(TotalHours))]
    public int TotalHours { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(ContestTypeId))]
    public Guid ContestTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(ContestTypeId))]
    public ContestType? ContestType { get; set; }
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    
    public ICollection<Game>? Games { get; set; }
    public ICollection<ContestPackage>? ContestPackages { get; set; }
    public ICollection<RolePreference>? RolePreferences { get; set; }
    public ICollection<ContestGameType>? ContestGameTypes { get; set; }
    public ICollection<ContestLevel>? ContestLevels { get; set; }
    public ICollection<ContestTime>? ContestTimes { get; set; }
    public ICollection<ContestTimeOfDay>? ContestTimeOfDays { get; set; }
    public ICollection<UserContestPackage>? UserContestPackages { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}