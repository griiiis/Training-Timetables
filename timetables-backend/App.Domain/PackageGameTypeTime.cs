using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class PackageGameTypeTime : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.PackageGameTypeTime), Name = nameof(PackageGtName))]
    [Column(TypeName = "jsonb")]
    public LangStr PackageGtName { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.PackageGameTypeTime), Name = nameof(GameTypeId))]
    public Guid GameTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.PackageGameTypeTime), Name = nameof(GameTypeId))]
    public GameType? GameType { get; set; }
    public decimal Times { get; set; }

    public ICollection<UserContestPackage>? UserContestPackages { get; set; }
    public ICollection<ContestPackage>? ContestPackages { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}