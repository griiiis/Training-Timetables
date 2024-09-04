 using App.Contracts.BLL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.ContestAdmin.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public GamesController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: Games
        public async Task<IActionResult> Index(Guid contestId)
        {
            var contest = _bll.Contests.FirstOrDefaultAsync(contestId).Result!;
            var teachers = (await _bll.UserContestPackages.GetContestTeachers(contestId)).ToList();
            var allDays = new List<DateTime>();
            for (var date = contest.From.Date; date <= contest.Until.Date; date = date.AddDays(1))
            {
                allDays.Add(date);
            }
            var vm = new GameIndexViewModel()
            {
                Games = (await _bll.Games.GetUserContestGames(contestId, UserId)).ToList(),
                Contest = contest,
                GameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(contestId)).ToList(),
                Days = allDays,
                Times = (await _bll.Times.GetAllCurrentContestAsync(contestId)).ToList(),
                Teachers = teachers,
                TeacherIds = teachers.Select(e => e.AppUserId).ToList()
            };
            return View(vm);
        }
    }
}