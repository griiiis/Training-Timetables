using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class PackageGameTypeTime : IDomainEntityId
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Domain.PackageGameTypeTime), Name = nameof(PackageGtName))]
    public LangStr PackageGtName { get; set; } = default!;
    
    public Guid GameTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.PackageGameTypeTime), Name = nameof(GameTypeId))]
    public GameType? GameType { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Domain.PackageGameTypeTime), Name = nameof(Times))]
    public decimal Times { get; set; }
}