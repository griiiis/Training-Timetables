using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Time : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.Time), Name = nameof(From))]
    public TimeOnly From { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Time), Name = nameof(Until))]
    public TimeOnly Until { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.Time), Name = nameof(TimeOfDayId))]
    public Guid TimeOfDayId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Time), Name = nameof(TimeOfDayId))]
    public TimeOfDay? TimeOfDay { get; set; }
    public ICollection<ContestTime>? ContestTimes { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}