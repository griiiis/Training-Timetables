using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class TeamGame : IDomainEntityId    
{
    public Guid TeamId { get; set; }
    
    public Guid GameId { get; set; }
    public Game? Game { get; set; }
    public Guid Id { get; set; }
}