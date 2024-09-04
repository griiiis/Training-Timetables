using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class GameIndexViewModel
{
    public List<Game> Games { get; set; } = default!;
    public Contest Contest { get; set; } = default!;
    public List<GameType> GameTypes { get; set; } = default!;
    public List<DateTime> Days { get; set; } = default!;
    public List<Time> Times { get; set; } = default!;
    public List<UserContestPackage> Teachers { get; set; } = default!;
    public List<Guid> TeacherIds { get; set; } = default!;
}