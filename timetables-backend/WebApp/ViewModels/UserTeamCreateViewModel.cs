using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class UserTeamCreateViewModel
{
    public Guid UserContestPackageId { get; set; }
    public IEnumerable<SelectListItem>? UserContestPackageSelectList { get; set; }
    public Guid TeamId { get; set; }
    public Guid ContestId { get; set; }
}