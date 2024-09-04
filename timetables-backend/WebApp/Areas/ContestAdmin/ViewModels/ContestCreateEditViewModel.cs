using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class ContestCreateEditViewModel
{
    public Contest Contest { get; set; } = default!;
    public SelectList? ContestTypeSelectList { get; set; }
    public SelectList? LocationSelectList { get; set; }
    public SelectList? LevelSelectList { get; set; }
    public SelectList? TimesSelectList { get; set; }
    public SelectList? PackagesSelectList { get; set; }
    
    public List<Level>? PreviousLevels { get; set; }
    public List<Time>? PreviousTimes { get; set; }
    public List<PackageGameTypeTime>? PreviousPackages { get; set; }
    
    public List<Guid>? SelectedLevelIds { get; set; }
    public List<Guid>? SelectedTimesIds { get; set; }
    public List<Guid>? SelectedPackagesIds { get; set; }
}