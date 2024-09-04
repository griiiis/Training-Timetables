using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Game : IDomainEntityId
{
    public Guid Id { get; set; }
    public LangStr Title { get; set; } = default!;
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    
    public Guid ContestId { get; set; }
    public Guid CourtId { get; set; }
    public Court? Court { get; set; }
    public Guid GameTypeId { get; set; }
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    public ICollection<TeamGame>? TeamGames { get; set; }
}