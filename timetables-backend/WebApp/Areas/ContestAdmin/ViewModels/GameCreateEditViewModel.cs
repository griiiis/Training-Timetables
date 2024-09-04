using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class GameCreateEditViewModel
{
    public List<UserContestPackage> UsersWithoutTrainers { get; set; } = default!;
    public List<UserContestPackage> Trainers { get; set; } = default!;
    public List<Time> Times { get; set; } = default!;
    public Contest Contest { get; set; } = default!;
    public List<Team> Teams { get; set; } = default!;
    public List<GameType> GameTypes { get; set; } = default!;
    public List<SelectListItem> LevelsSelectList { get; set; } = default!;
    public List<Court> Courts { get; set; } = default!;
    public Dictionary<Guid, int> PeopleCountPerGameType { get; set; } = default!;
    public string? SelectedTimes { get; set; }
    public List<GameTypeViewModel> GameTypesModel { get; set; } = new ();

}

public class GameTypeViewModel
{
    public Guid GameTypeId { get; set; }
    public int PeopleOnCourt { get; set; }
    public List<Guid>? SelectedLevelsList { get; set; }
    public List<Guid>? SelectedCourtsList { get; set; }
    public List<Guid>? SelectedTeachersList { get; set; }
}

public class TimeOfDaysViewModel
{
    public DateOnly? Date { get; set; }
    public List<Guid>? SelectedTimesList { get; set; }
}