using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Level : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.Level), Name = nameof(Title))]
    [Column(TypeName = "jsonb")]
    public LangStr Title { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.Level), Name = nameof(Description))]
    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = default!;

    public ICollection<Game>? Games { get; set; }
    public ICollection<RolePreference>? RolePreferences { get; set; }
    public ICollection<Team>? Teams { get; set; }
    public ICollection<ContestLevel>? ContestLevels { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
