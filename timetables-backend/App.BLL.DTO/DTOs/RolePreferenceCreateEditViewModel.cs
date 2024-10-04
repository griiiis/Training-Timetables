namespace App.BLL.DTO.Models;

public class RolePreferenceViewModel
{
    public List<List<string>> SelectedLevelsList { get; set; } = default!;
    public string ContestId { get; set; } = default!;
}