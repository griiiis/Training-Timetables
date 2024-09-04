using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class TimeCreateEditViewModel
{
    public Time Time { get; set; } = default!;
    public SelectList? TimeOfDaySelectList { get; set; }
}