using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class ContestType : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.ContestType), Name = nameof(ContestTypeName))]
    [Column(TypeName = "jsonb")]
    public LangStr ContestTypeName { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.ContestType), Name = nameof(Description))]
    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = default!;

    public ICollection<Contest>? Contests { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}