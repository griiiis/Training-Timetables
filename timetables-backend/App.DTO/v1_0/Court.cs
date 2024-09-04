using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Court : IDomainEntityId   
{
    public string CourtName { get; set; } = default!;
    
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    
    public Guid Id { get; set; }
}