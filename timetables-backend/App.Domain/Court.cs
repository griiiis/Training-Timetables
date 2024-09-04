using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Court : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.Court), Name = nameof(CourtName))]
    [Column(TypeName = "jsonb")]
    public LangStr CourtName { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.Court), Name = nameof(GameTypeId))]
    public Guid GameTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Court), Name = nameof(GameTypeId))]
    public GameType? GameType { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(Location))]
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }

    public ICollection<Game>? Games { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
}