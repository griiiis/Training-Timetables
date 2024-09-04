using System.Diagnostics;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ContestsController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ContestsController(IAppBLL bll, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _bll = bll;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Contests
        public async Task<IActionResult> Index(ContestIndexViewModel indexVm)
        {
            var allContests = (await _bll.Contests.GetAllAsync(default)).ToList();

            var searchedBooks = _bll.Contests.GetAllAsync(default).Result.ToList();
            
            if (!string.IsNullOrWhiteSpace(indexVm.Search))
            {
                indexVm.Search = indexVm.Search.ToUpper();

                if (indexVm.ContestType)
                {
                    searchedBooks =
                        searchedBooks.Where(e => e.ContestType!.ContestTypeName.ToUpper().Contains(indexVm.Search)).ToList();
                }
                if (indexVm.Location)
                {
                    searchedBooks =
                        searchedBooks.Where(e => e.Location!.LocationName.ToString().ToUpper().Contains(indexVm.Search)).ToList();
                }
                if (indexVm.GameType)
                {
                    searchedBooks =
                        searchedBooks.Where(e => e.ContestGameTypes!.Any(c => c.GameType!.GameTypeName.ToString().ToUpper().Contains(indexVm.Search))).ToList();
                }
            }
            

            var vm = new ContestIndexViewModel
            {
                Search = indexVm.Search?.ToLower(),
                SearchedContests = searchedBooks.Take(4).ToList(),
                CurrentContests = new List<ContestIndexViewModel.ContestViewModel>(),
                ComingContests = new List<ContestIndexViewModel.ContestViewModel>()
            };

            var currentContests = allContests.Where(e => e.From < DateTime.Now && e.Until > DateTime.Now).Take(2)
                .ToList();
            foreach (var contest in currentContests)
            {
                var contestVm = new ContestIndexViewModel.ContestViewModel
                {
                    Contest = contest,
                    GameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(contest.Id)).ToList(),
                    NumberOfParticipants = _bll.UserContestPackages.GetContestUsersWithoutTeachers(contest.Id).Result
                        .Count(),
                    ifAlreadyJoined = false,
                };
                vm.CurrentContests.Add(contestVm);
            }
            
            var comingContests = allContests.Where(e => e.From > DateTime.Now).ToList()
                .ToList();
            foreach (var contest in comingContests)
            {
                var contestVm = new ContestIndexViewModel.ContestViewModel
                {
                    Contest = contest,
                    GameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(contest.Id)).ToList(),
                    NumberOfParticipants = _bll.UserContestPackages.GetContestUsersWithoutTeachers(contest.Id).Result
                        .Count(),
                    ifAlreadyJoined = _signInManager.IsSignedIn(User) && _bll.UserContestPackages.IfAlreadyJoined(contest.Id, Guid.Parse(_userManager.GetUserId(User)!)),
                };
                vm.ComingContests.Add(contestVm);
            }
            return View(vm);
        }

        // GET: Contests
        public async Task<IActionResult> MyContests()
        {
            var userId = Guid.Parse(_userManager.GetUserId(User)!);
            var allContests = (await _bll.Contests.GetUserContests(userId)).ToList();

            List<RolePreference> rolePreferences = (await _bll.RolePreferences.GetAllAsync(userId)).ToList();

            var vm = new ContestMyContestsViewModel
            {
                ComingContests = new List<ContestMyContestsViewModel.ContestViewModel>(),
                CurrentContests = new List<ContestMyContestsViewModel.ContestViewModel>(),
                EndedContests = allContests.Where(e => e.Until < DateTime.Now).ToList(),
                RolePreferences = rolePreferences,
            };

            //Current contests
            var currentContests = allContests.Where(e => e.From < DateTime.Now && e.Until > DateTime.Now).ToList();
            foreach (var contest in currentContests)
            {
                var userContestPackage = _bll.UserContestPackages.GetUserContestPackage(contest.Id, userId).Result!;

                var contestVm = new ContestMyContestsViewModel.ContestViewModel
                {
                    Contest = _bll.Contests.FirstOrDefaultAsync(contest.Id).Result!,
                    AnyGames = _bll.Games.AnyContestGames(contest.Id),
                    IfTrainer = _bll.ContestUserRoles.IfContestTrainer(userId, contest.Id),
                    UserId = userId,
                    UserContestPackage = userContestPackage,
                    UserContestPackages = _bll.UserContestPackages
                        .GetContestUsersWithoutTeachers(contest.Id, userContestPackage.TeamId).Result.ToList(),
                    GameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(contest.Id)).ToList(),
                    Level = _bll.Levels.FirstOrDefaultAsync(userContestPackage.LevelId).Result!,
                    GameType = _bll.GameTypes.FirstOrDefaultAsync(userContestPackage.PackageGameTypeTime!.GameTypeId)
                        .Result!,
                    PackageGameTypeTime = _bll.PackageGameTypeTimes
                        .FirstOrDefaultAsync(userContestPackage.PackageGameTypeTimeId).Result!
                };
                vm.CurrentContests.Add(contestVm);
            }

            //Coming contests
            var comingContests = allContests.Where(e => e.From > DateTime.Now).ToList();
            foreach (var contest in comingContests)
            {
                var userContestpackage = _bll.UserContestPackages.GetUserContestPackage(contest.Id, userId).Result!;

                var contestVm = new ContestMyContestsViewModel.ContestViewModel
                {
                    Contest = _bll.Contests.FirstOrDefaultAsync(contest.Id).Result!,
                    AnyGames = _bll.Games.AnyContestGames(contest.Id),
                    UserId = userId,
                    IfTrainer = _bll.ContestUserRoles.IfContestTrainer(userId, contest.Id),
                    UserContestPackage = userContestpackage,
                    UserContestPackages = _bll.UserContestPackages
                        .GetContestUsersWithoutTeachers(contest.Id, userContestpackage!.TeamId).Result.ToList(),
                    GameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(contest.Id)).ToList(),
                    Level = _bll.Levels.FirstOrDefaultAsync(userContestpackage.LevelId).Result!,
                    GameType = _bll.GameTypes.FirstOrDefaultAsync(userContestpackage.PackageGameTypeTime!.GameTypeId)
                        .Result!,
                    PackageGameTypeTime = _bll.PackageGameTypeTimes
                        .FirstOrDefaultAsync(userContestpackage.PackageGameTypeTimeId).Result!
                };
                vm.ComingContests.Add(contestVm);
            }

            return View(vm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
            );
            return LocalRedirect(returnUrl);
        }
    }
}