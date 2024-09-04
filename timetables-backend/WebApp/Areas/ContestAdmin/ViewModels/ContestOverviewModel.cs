using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class ContestOverviewViewModel
{
    public Contest Contest { get; set; } = default!;
    public List<Team> Teams { get; set; } = default!;
    public List<UserContestPackage> UserContestPackages { get; set; } = default!;
    public List<GameType> GameTypes { get; set; } = default!;
    public List<UserContestPackage> Teachers { get; set; } = default!;
    public List<Level> Levels { get; set; } = default!;
}