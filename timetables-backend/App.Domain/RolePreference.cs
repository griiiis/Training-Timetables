using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class RolePreference : BaseEntityId, IDomainAppUser<AppUser>
{
    [Display(ResourceType = typeof(App.Resources.Domain.RolePreference), Name = nameof(LevelId))]
    public Guid LevelId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.RolePreference), Name = nameof(LevelId))]
    public Level? Level { get; set; }

    [Display(ResourceType = typeof(App.Resources.Domain.RolePreference), Name = nameof(GameTypeId))]
    public Guid GameTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.RolePreference), Name = nameof(GameTypeId))]
    public GameType? GameType { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }

    public int GamesTrainedHourly { get; set; }
}