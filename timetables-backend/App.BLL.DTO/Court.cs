using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Court : IDomainEntityId   
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Court), Name = nameof(CourtName))]
    public LangStr CourtName { get; set; } = default!;
    
    public Guid GameTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Court), Name = nameof(GameTypeId))]
    public GameType? GameType { get; set; }
    
    public Guid LocationId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Court), Name = nameof(Location))]
    public Location? Location { get; set; }
}