using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Team : IDomainEntityId
{
    public string TeamName { get; set; } = default!;
    
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }

    public ICollection<TeamGame>? TeamGames { get; set; }
    public Guid Id { get; set; }
}