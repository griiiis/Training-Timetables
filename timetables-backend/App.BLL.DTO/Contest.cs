using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Contest : IDomainEntityId
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(ContestName))]
    public LangStr ContestName { get; set; } = default!;
    public LangStr Description { get; set; } = default!;
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    public int TotalHours { get; set; }
    
    public Guid ContestTypeId { get; set; }
    public ContestType? ContestType { get; set; }
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    
    public ICollection<ContestGameType>? ContestGameTypes { get; set; }
    public ICollection<ContestPackage>? ContestPackages { get; set; }
    public ICollection<ContestLevel>? ContestLevels { get; set; }
    public ICollection<ContestTime>? ContestTimes { get; set; }
}