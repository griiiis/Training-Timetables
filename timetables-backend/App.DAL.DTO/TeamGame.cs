using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class TeamGame : IDomainEntityId    
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    public Guid GameId { get; set; }
    public Game? Game { get; set; }
}