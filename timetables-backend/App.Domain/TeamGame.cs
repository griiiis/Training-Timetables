using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class TeamGame : BaseEntityId    
{
    [Display(ResourceType = typeof(App.Resources.Domain.TeamGame), Name = nameof(TeamId))]
    public Guid TeamId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.TeamGame), Name = nameof(TeamId))]
    public Team? Team { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.TeamGame), Name = nameof(GameId))]
    public Guid GameId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.TeamGame), Name = nameof(GameId))]
    public Game? Game { get; set; }
}