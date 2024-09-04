using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Game : BaseEntityId
{
    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(Title))]
    [Column(TypeName = "jsonb")]
    public LangStr Title { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(From))]
    public DateTime From { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(Until))]
    public DateTime Until { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(ContestId))]
    public Guid ContestId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(ContestId))]
    public Contest? Contest { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(CourtId))]
    public Guid CourtId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(CourtId))]
    public Court? Court { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(GameTypeId))]
    public Guid GameTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(GameTypeId))]
    public GameType? GameType { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(LevelId))]
    public Guid LevelId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Game), Name = nameof(LevelId))]
    public Level? Level { get; set; }

    public ICollection<TeamGame>? TeamGames { get; set; }
}