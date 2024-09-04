using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.ContestAdmin.ViewModels;

public class CourtCreateEditViewModel
{
    public Court Court { get; set; } = default!;
    public SelectList? GameTypeSelectList { get; set; }
    public SelectList? LocationSelectList { get; set; }
}