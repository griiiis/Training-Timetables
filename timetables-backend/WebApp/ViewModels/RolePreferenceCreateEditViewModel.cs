using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class RolePreferenceCreateEditViewModel
{
    public List<GameType>? GameTypes { get; set; }
    public List<List<Guid>> SelectedLevelsList { get; set; } = new List<List<Guid>>();
    public SelectList? LevelSelectList { get; set; }
    public string? ContestId { get; set; } = default!;
    public List<RolePreference>? PreviousRolePreferences { get; set; }
}