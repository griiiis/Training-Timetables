using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Game : IDomainEntityId
{
    public string Title { get; set; } = default!;
    
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    
    public Guid ContestId { get; set; }
    
    public Guid CourtId { get; set; }
    public Court? Court { get; set; }
    
    public Guid GameTypeId { get; set; }
    
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    public Guid Id { get; set; }
}