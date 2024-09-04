using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class TimeTeam : BaseEntityId        
{
    [Display(ResourceType = typeof(App.Resources.Domain.TimeTeam), Name = nameof(Team))]
    public Guid TeamId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.TimeTeam), Name = nameof(Team))]
    public Team? Team { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.TimeTeam), Name = nameof(TimeOfDay))]
    public Guid TimeOfDayId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.TimeTeam), Name = nameof(TimeOfDay))]
    public TimeOfDay? TimeOfDay { get; set; }

    public DateOnly Day { get; set; }
}