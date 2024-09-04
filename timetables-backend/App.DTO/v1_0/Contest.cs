using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Contest : IDomainEntityId
{
    public Guid Id { get; set; }
    public string ContestName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TotalHours { get; set; }
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    
    public Guid ContestTypeId { get; set; }
    public ContestType? ContestType { get; set; }
    
    public ICollection<ContestGameType>? ContestGameTypes { get; set; }
    public ICollection<UserContestPackage>? UserContestPackages { get; set; }
    
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
}