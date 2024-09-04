using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class TimeOfDay : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.TimeOfDay), Name = nameof(TimeOfDayName))]
    [Column(TypeName = "jsonb")]
    public LangStr TimeOfDayName { get; set; } = default!;
    
    public ICollection<Time>? Times { get; set; }
    public ICollection<TimeTeam>? TimeTeams { get; set; }
    public ICollection<ContestTimeOfDay>? ContestTimeOfDays { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}