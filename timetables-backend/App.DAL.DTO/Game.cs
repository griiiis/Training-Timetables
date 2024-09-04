using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class Game : IDomainEntityId
{
    public LangStr Title { get; set; } = default!;
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    public Guid CourtId { get; set; }
    public Court? Court { get; set; }
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    public ICollection<TeamGame>? TeamGames { get; set; }
    public Guid Id { get; set; }
}