using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.DTO.v1_0;

public class RolePreferenceViewModel
{
    public List<List<string>> SelectedLevelsList { get; set; } = default!;
    public string ContestId { get; set; } = default!;
}