using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class GameType : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.GameType), Name = nameof(GameTypeName))]
    [Column(TypeName = "jsonb")]
    public LangStr GameTypeName { get; set; } = default!;

    public ICollection<Court>? Courts { get; set; }
    public ICollection<Game>? Games { get; set; }
    public ICollection<Team>? Teams { get; set; }
    public ICollection<RolePreference>? RolePreferences { get; set; }
    public ICollection<PackageGameTypeTime>? PackageGameTypeTimes { get; set; }
    public ICollection<ContestGameType>? ContestGameTypes { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}