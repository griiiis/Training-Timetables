using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class UserContestPackageCreateViewModel
{
    public UserContestPackage UserContestPackage { get; set; } = default!;
    public SelectList? PackageGameTypeTimeSelectList { get; set; }
    public Contest? Contest { get; set; }
    public SelectList? LevelSelectList { get; set; }
}