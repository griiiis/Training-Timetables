using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Team : BaseEntityId
{
    [Display(ResourceType = typeof(App.Resources.Domain.Team), Name = nameof(TeamName))]
    [Column(TypeName = "jsonb")]
    public LangStr TeamName { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.Team), Name = nameof(LevelId))]
    public Guid LevelId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Team), Name = nameof(LevelId))]
    public Level? Level { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Team), Name = nameof(GameTypeId))]
    public Guid GameTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Team), Name = nameof(GameTypeId))]
    public GameType? GameType { get; set; }

    public ICollection<UserContestPackage>? UserContestPackages { get; set; }
    public ICollection<TeamGame>? TeamGames { get; set; }
    public ICollection<TimeTeam>? TimeTeams { get; set; }
}