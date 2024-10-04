using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.BLL.DTO;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;
using Contest = App.Domain.Contest;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserContestPackagesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;

        public UserContestPackagesController(IAppBLL bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }
        
        // GET: UserContestPackages/Create
        public async Task<IActionResult> Create(Guid contestId)
        {
            var vm = new UserContestPackageCreateViewModel()
            { 
                LevelSelectList = new SelectList(await _bll.Levels.GetAllCurrentContestAsync(contestId),nameof(Level.Id),
                    nameof(Level.Title)),
                PackageGameTypeTimeSelectList = new SelectList(await _bll.PackageGameTypeTimes.GetAllCurrentContestAsync(contestId), nameof(PackageGameTypeTime.Id),
                    nameof(PackageGameTypeTime.PackageGtName)),
                Contest = await _bll.Contests.FirstOrDefaultAsync(contestId),
            };
            return View(vm);
        }

        // POST: UserContestPackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserContestPackageCreateViewModel vm)
        {
            var userId = Guid.Parse(_userManager.GetUserId(User)!);
            if (ModelState.IsValid)
            {
                //Osavõtja roll
                var participantRoleId = await _bll.ContestRoles.ContestRoleId("Participant");
                var participantUserRole = new ContestUserRole
                {
                    ContestRoleId = participantRoleId,
                    AppUserId = userId
                };
                _bll.ContestUserRoles.Add(participantUserRole);


                var packageGameType = await _bll.PackageGameTypeTimes.FirstOrDefaultAsync(vm.UserContestPackage.PackageGameTypeTimeId);
                var contest = await _bll.Contests.FirstOrDefaultAsync(vm.UserContestPackage.ContestId);
                var level = await _bll.Levels.FirstOrDefaultAsync(vm.UserContestPackage.LevelId);
                
                var team = new Team()  // Create team
                {
                    Id = Guid.NewGuid(),
                    TeamName = _userManager.GetUserName(User) + " team",
                    LevelId = level!.Id,
                    GameTypeId = packageGameType!.GameTypeId,
                };
                _bll.Teams.Add(team);
                
                vm.UserContestPackage.Id = Guid.NewGuid();
                vm.UserContestPackage.HoursAvailable = contest!.TotalHours * packageGameType.Times;
                vm.UserContestPackage.AppUserId = userId;
                vm.UserContestPackage.TeamId = team.Id;
                _bll.UserContestPackages.Add(vm.UserContestPackage);
                await _bll.SaveChangesAsync();
                
                /*
                //Create all times for team
                var timeOfDays = _bll.TimeOfDays.GetAllAsync().Result.ToList();
                var contestStartDate = contest.From.Date;
                var contestEndDate = contest.Until.Date;
                var contestLength = (contestEndDate - contestStartDate).TotalDays;

                for (int i = 0; i <= contestLength; i++)
                {
                    foreach (var timeOfDay in timeOfDays)
                    {
                        _bll.TimeTeams.Add(new TimeTeam
                        {
                            TimeOfDayId = timeOfDay.Id,
                            Day = DateOnly.FromDateTime (contestStartDate.AddDays(i)),
                            TeamId = team.Id,
                        });
                    }
                }
                */
                
                await _bll.SaveChangesAsync();
                return RedirectToAction("MyContests", "Contests");
            }
            vm.PackageGameTypeTimeSelectList = new SelectList(await _bll.PackageGameTypeTimes.GetAllAsync(userId), nameof(PackageGameTypeTime.Id),
                nameof(PackageGameTypeTime.PackageGtName));
            vm.Contest = await _bll.Contests.FirstOrDefaultAsync(vm.UserContestPackage.ContestId);
            vm.LevelSelectList = new SelectList(await _bll.Levels.GetAllCurrentContestAsync(vm.UserContestPackage.ContestId), nameof(Level.Id),
                nameof(Level.Title));
            return View(vm);
        }
        
        // GET: UserContestPackages/Invite
        public async Task<IActionResult> Invite(Guid contestId, Guid teamId)
        {
            var users = _bll.UserContestPackages.GetContestTeammates(contestId, teamId).Result
                .Select(e => e.AppUserId).ToList();
            
            var vm = new UserTeamCreateViewModel()
            {
                ContestId = contestId,
                TeamId = teamId,
                UserContestPackageSelectList = (await _bll.UserContestPackages.GetContestParticipants(contestId))
                    .Where(e => !users.Contains(e.AppUserId))
                    .Select(e => new SelectListItem() {Text = e.AppUser!.FirstName + ' ' + e.AppUser!.LastName , Value = e.Id.ToString()}).ToList()
            };
            return View(vm);
        }

        // POST: UserContestPackages/Invite
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(UserTeamCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var package = await _bll.UserContestPackages.FirstOrDefaultAsync(vm.UserContestPackageId);
                var appUserId = package!.AppUserId;

                var newPackage = new UserContestPackage
                {
                    PackageGameTypeTimeId = package.PackageGameTypeTimeId,
                    HoursAvailable = 0,
                    ContestId = package.ContestId,
                    TeamId = vm.TeamId,
                    LevelId = package.LevelId,
                };

                _bll.UserContestPackages.AddPackageWithUser(appUserId, newPackage);

                //Get Previous Team
                var previousTeamId =
                    _bll.UserContestPackages.GetUserContestPackage(vm.ContestId, appUserId).Result!.TeamId;
                
                //Delete previous TimeTeams
                await _bll.TimeTeams.RemoveTeamTimeTeamsAsync(previousTeamId);
                
                //Delete previous package
                await _bll.UserContestPackages.RemoveAsync(package);
                
                //Delete previous Team
                await _bll.Teams.RemoveAsync(previousTeamId);
                
                await _bll.SaveChangesAsync();
                return RedirectToAction("MyContests", "Contests");
            }

            var userId = _bll.UserContestPackages.FirstOrDefaultAsync(vm.UserContestPackageId).Result!.AppUserId;
            vm.UserContestPackageSelectList = new SelectList(await _bll.UserContestPackages.GetAllAsync(userId),
                nameof(UserContestPackage.Id),
                nameof(UserContestPackage.AppUser));
            vm.ContestId = vm.ContestId;
            vm.TeamId = vm.TeamId;
            return RedirectToAction("MyContests", "Contests");
        }
/*
        // GET: UserContestPackages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userContestPackage = await _context.UserContestPackages.FindAsync(id);
            if (userContestPackage == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", userContestPackage.AppUserId);
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Description", userContestPackage.ContestId);
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Description", userContestPackage.LevelId);
            ViewData["PackageGameTypeTimeId"] = new SelectList(_context.PackageGameTypeTimes, "Id", "PackageGtName", userContestPackage.PackageGameTypeTimeId);
            return View(userContestPackage);
        }

        // POST: UserContestPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PackageGameTypeTimeId,HoursAvailable,AppUserId,ContestId,LevelId,Id")] UserContestPackage userContestPackage)
        {
            if (id != userContestPackage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userContestPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserContestPackageExists(userContestPackage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", userContestPackage.AppUserId);
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Description", userContestPackage.ContestId);
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Description", userContestPackage.LevelId);
            ViewData["PackageGameTypeTimeId"] = new SelectList(_context.PackageGameTypeTimes, "Id", "PackageGtName", userContestPackage.PackageGameTypeTimeId);
            return View(userContestPackage);
        }

        // GET: UserContestPackages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userContestPackage = await _context.UserContestPackages
                .Include(u => u.AppUser)
                .Include(u => u.Contest)
                .Include(u => u.Level)
                .Include(u => u.PackageGameTypeTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userContestPackage == null)
            {
                return NotFound();
            }

            return View(userContestPackage);
        }

        // POST: UserContestPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userContestPackage = await _context.UserContestPackages.FindAsync(id);
            if (userContestPackage != null)
            {
                _context.UserContestPackages.Remove(userContestPackage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserContestPackageExists(Guid id)
        {
            return _context.UserContestPackages.Any(e => e.Id == id);
        }
        */
    }
}
