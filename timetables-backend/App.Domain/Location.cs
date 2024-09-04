using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Location : BaseEntityId, IDomainAppUser<AppUser>

{
    [Display(ResourceType = typeof(App.Resources.Domain.Location), Name = nameof(LocationName))]
    [Column(TypeName = "jsonb")]
    public LangStr LocationName { get; set; } = default!;
    
    [Display(ResourceType = typeof(App.Resources.Domain.Location), Name = nameof(State))]
    [Column(TypeName = "jsonb")]
    public LangStr State { get; set; } = default!;
    
    [Display(ResourceType = typeof(App.Resources.Domain.Location), Name = nameof(Country))]
    [Column(TypeName = "jsonb")]
    public LangStr Country { get; set; } = default!;

    public ICollection<Contest>? Contests { get; set; }
    public ICollection<Court>? Courts { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}