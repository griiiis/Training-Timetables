using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Team : IDomainEntityId
{
    public Guid Id { get; set; }
    public LangStr TeamName { get; set; } = default!;
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    public ICollection<UserContestPackage>? UserContestPackages { get; set; }
    public ICollection<TeamGame>? TeamGames { get; set; }
}