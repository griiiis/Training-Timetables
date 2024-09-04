using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class PackageGameTypeTimeCreateEditViewModel
{
    public PackageGameTypeTime PackageGameTypeTime { get; set; } = default!;
    public SelectList? GameTypeSelectList { get; set; }
}