using App.BLL.DTO;
using App.Contracts.BLL;

using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.ContestAdmin.ViewModels;

namespace WebApp.Areas.ContestAdmin.Controllers
{
    [Authorize(Roles = "Contest Admin")]
    [Area("ContestAdmin")]
    public class ContestsController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public ContestsController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }


        // GET: Contests
        public async Task<IActionResult> Index()
        {
            var contest = await _bll.Contests.GetAllAsync(UserId);

            foreach (var contestInfo in contest)
            {
                contestInfo.From = contestInfo.From.ToUniversalTime();
                contestInfo.Until = contestInfo.Until.ToUniversalTime();
            }

            return View(contest);
        }
        // GET: Contests/Overview
        public async Task<IActionResult> OverView(Guid contestId)
        {
            var contest = await _bll.Contests.FirstOrDefaultAsync(contestId);
            contest!.From = contest.From.ToUniversalTime();
            contest.Until = contest.Until.ToUniversalTime();

            var vm = new ContestOverviewViewModel
            {
                Contest = contest,
                Teams = _bll.Teams.GetAllCurrentContestAsync(contest.Id).Result.ToList(),
                UserContestPackages =
                    _bll.UserContestPackages.GetContestUsersWithoutTeachers(contest.Id).Result.ToList(),
                GameTypes = _bll.GameTypes.GetAllCurrentContestAsync(contest.Id).Result.ToList(),
                Teachers = _bll.UserContestPackages.GetContestTeachers(contest.Id).Result.ToList(),
                Levels = _bll.Levels.GetAllCurrentContestAsync(contest.Id).Result.ToList(),
            };
            
            return View(vm);
        }

        // GET: Contests/Create
        public async Task<IActionResult> Create()
        {
            var times = await _bll.Times.GetAllAsync(UserId);
            var vm = new ContestCreateEditViewModel()
            {
                ContestTypeSelectList = new SelectList(
                    await _bll.ContestTypes.GetAllAsync(UserId),
                    nameof(ContestType.Id),
                    nameof(ContestType.ContestTypeName)),
                LocationSelectList = new SelectList(
                    await _bll.Locations.GetAllAsync(UserId), nameof(Location.Id),
                    nameof(Location.LocationName)),
                LevelSelectList = new SelectList(
                    await _bll.Levels.GetAllAsync(UserId), nameof(Level.Id),
                    nameof(Level.Title)),
                TimesSelectList = new SelectList(times.Select(t => new {t.Id, DisplayText = $"{t.From} - {t.Until}" }), "Id", "DisplayText"),
                PackagesSelectList =
                    new SelectList(
                        await _bll.PackageGameTypeTimes.GetAllAsync(UserId),
                        nameof(PackageGameTypeTime.Id), nameof(PackageGameTypeTime.PackageGtName))
            };
            return View(vm);
        }

        // POST: Contests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContestCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.SelectedLevelIds!.Count < 1 || vm.SelectedTimesIds!.Count < 1 ||
                    vm.SelectedPackagesIds!.Count < 1)
                {
                    return View(vm);
                }

                vm.Contest.Id = Guid.NewGuid();
                vm.Contest.From = vm.Contest.From.ToUniversalTime();
                vm.Contest.Until = vm.Contest.Until.ToUniversalTime();

                foreach (var levelId in vm.SelectedLevelIds!)
                {
                    var contestLevel = new ContestLevel
                    {
                        ContestId = vm.Contest.Id,
                        LevelId = levelId
                    };
                    _bll.ContestLevels.Add(contestLevel);
                }

                var gameTypes = new HashSet<Guid>();
                var allPackages = (await _bll.PackageGameTypeTimes.GetAllAsync(default)).ToList();

                foreach (var id in vm.SelectedPackagesIds)
                {
                    var gameTypeId = allPackages.FirstOrDefault(e => e.Id.Equals(id) && !gameTypes.Contains(e.GameTypeId))?.GameTypeId;
    
                    if (gameTypeId != null)
                    {
                        gameTypes.Add(gameTypeId.Value);
                    }
                }
                
                foreach (var gameTypeId in gameTypes)
                {
                    var contestGameType = new ContestGameType()
                    {
                        ContestId = vm.Contest.Id,
                        GameTypeId = gameTypeId
                    };
                    _bll.ContestGameTypes.Add(contestGameType);
                }

                foreach (var timeId in vm.SelectedTimesIds!)
                {
                    var times = new ContestTime()
                    {
                        ContestId = vm.Contest.Id,
                        TimeId = timeId
                    };
                    _bll.ContestTimes.Add(times);
                }

                foreach (var packageId in vm.SelectedPackagesIds!)
                {
                    var package = new ContestPackage()
                    {
                        ContestId = vm.Contest.Id,
                        PackageGameTypeTimeId = packageId
                    };
                    _bll.ContestPackages.Add(package);
                }
                 
                //Contest Roles by default
                var trainerRole = new ContestRole
                {
                    ContestRoleName = "Trainer",
                    ContestId = vm.Contest.Id
                };

                var participantRole = new ContestRole()
                {
                    ContestRoleName = "Participant",
                    ContestId = vm.Contest.Id
                };
                _bll.ContestRoles.Add(participantRole);
                _bll.ContestRoles.Add(trainerRole);
                
                
                _bll.Contests.AddContestWithUser(UserId, vm.Contest);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.ContestTypeSelectList = new SelectList(
                await _bll.ContestTypes.GetAllAsync(UserId), nameof(ContestType.Id),
                nameof(ContestType.ContestTypeName));
            vm.LocationSelectList = new SelectList(
                await _bll.Locations.GetAllAsync(UserId), nameof(Location.Id),
                nameof(Location.LocationName));
            vm.LevelSelectList = new SelectList(await _bll.Levels.GetAllAsync(UserId),
                nameof(Level.Id),
                nameof(Level.Title));
            vm.TimesSelectList =
                new SelectList(await _bll.Times.GetAllAsync(UserId),
                    nameof(TimeOfDay.Id), nameof(TimeOfDay.TimeOfDayName));
            vm.PackagesSelectList =
                new SelectList(await _bll.PackageGameTypeTimes.GetAllAsync(UserId),
                    nameof(PackageGameTypeTime.Id), nameof(PackageGameTypeTime.PackageGtName));
            return View(vm);
        }

        // GET: Contests/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null || !_bll.Contests.IsContestOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var contest = await _bll.Contests.FirstOrDefaultAsync(id, UserId);
            if (contest == null)
            {
                return NotFound();
            }
            
            var previousLevels = (await _bll.Contests.FirstOrDefaultAsync(id, UserId))!.ContestLevels.Select(e => e.Level).ToList();
            var previousPackages = (await _bll.Contests.FirstOrDefaultAsync(id, UserId))!.ContestPackages.Select(e => e.PackageGameTypeTime).ToList();
            var previousTimes = (await _bll.Contests.FirstOrDefaultAsync(id, UserId))!.ContestTimes.Select(e => e.Time).ToList();

            var vm = new ContestCreateEditViewModel()
            {
                Contest = contest,
                ContestTypeSelectList = new SelectList(
                    await _bll.ContestTypes.GetAllAsync(UserId),
                    nameof(ContestType.Id),
                    nameof(ContestType.ContestTypeName)),
                LocationSelectList = new SelectList(
                    await _bll.Locations.GetAllAsync(UserId), nameof(Location.Id),
                    nameof(Location.LocationName)),
                LevelSelectList = new SelectList(
                    await _bll.Levels.GetAllAsync(UserId),
                    nameof(Level.Id),
                    nameof(Level.Title)),
                TimesSelectList = 
                    new SelectList(await _bll.Times.GetAllAsync(UserId),
                        nameof(Time.Id), nameof(Time.From)),
                PackagesSelectList =
                    new SelectList(
                        await _bll.PackageGameTypeTimes.GetAllAsync(UserId),
                        nameof(PackageGameTypeTime.Id), nameof(PackageGameTypeTime.PackageGtName)),
                PreviousLevels = previousLevels!,
                PreviousPackages = previousPackages!,
                PreviousTimes = previousTimes!,
            };
            return View(vm);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ContestCreateEditViewModel vm)
        {
            if (id != vm.Contest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbEntry = _bll.Contests.FirstOrDefaultAsync(id).Result!;
                    var str = vm.Contest.ContestName;
                    vm.Contest.ContestName = dbEntry.ContestName;
                    vm.Contest.ContestName.SetTranslation(str);
                    
                    
                    //Remove previous packages
                    var previousPackages = _bll.ContestPackages.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                    foreach (var package in previousPackages)
                    {
                        await _bll.ContestPackages.RemoveAsync(package);
                    }
                    //Remove previous gameTypes
                    var previousGameTypes = _bll.ContestGameTypes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                    foreach (var gameType in previousGameTypes)
                    {
                        await _bll.ContestGameTypes.RemoveAsync(gameType);
                    }
                    //Remove times
                    var previousTimes = _bll.ContestTimes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                    foreach (var time in previousTimes)
                    {
                        await _bll.ContestTimes.RemoveAsync(time);
                    }
                    //Remove levels
                    var previousLevels = _bll.ContestLevels.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                    foreach (var level in previousLevels)
                    {
                        await _bll.ContestLevels.RemoveAsync(level);
                    }
                    
                    var gameTypes = new HashSet<Guid>();
                    var allPackages = (await _bll.PackageGameTypeTimes.GetAllAsync(default)).ToList();

                    foreach (var packageId in vm.SelectedPackagesIds!)
                    {
                        var gameTypeId = allPackages.FirstOrDefault(e => e.Id.Equals(packageId) && !gameTypes.Contains(e.GameTypeId))?.GameTypeId;
    
                        if (gameTypeId != null)
                        {
                            gameTypes.Add(gameTypeId.Value);
                        }
                    }
                
                    foreach (var gameTypeId in gameTypes)
                    {
                        var contestGameType = new ContestGameType()
                        {
                            ContestId = vm.Contest.Id,
                            GameTypeId = gameTypeId
                        };
                        _bll.ContestGameTypes.Add(contestGameType);
                    }

                    foreach (var timeId in vm.SelectedTimesIds!)
                    {
                        var times = new ContestTime()
                        {
                            ContestId = vm.Contest.Id,
                            TimeId = timeId
                        };
                        _bll.ContestTimes.Add(times);
                    }

                    foreach (var packageId in vm.SelectedPackagesIds!)
                    {
                        var package = new ContestPackage()
                        {
                            ContestId = vm.Contest.Id,
                            PackageGameTypeTimeId = packageId
                        };
                        _bll.ContestPackages.Add(package);
                    }
                    foreach (var levelId in vm.SelectedLevelIds!)
                    {
                        var contestLevel = new ContestLevel
                        {
                            ContestId = vm.Contest.Id,
                            LevelId = levelId
                        };
                        _bll.ContestLevels.Add(contestLevel);
                    }
                    
                    _bll.Contests.UpdateContestWithUser(UserId, vm.Contest);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.Contests.ExistsAsync(vm.Contest.Id))
                    {
                        return NotFound();
                    }

                    throw new Exception();
                }

                return RedirectToAction("");
            }

            vm.ContestTypeSelectList = new SelectList(
                await _bll.ContestTypes.GetAllAsync(UserId), nameof(ContestType.Id),
                nameof(ContestType.ContestTypeName));
            vm.LocationSelectList = new SelectList(
                await _bll.Locations.GetAllAsync(UserId), nameof(Location.Id),
                nameof(Location.LocationName));
            vm.LevelSelectList = new SelectList(await _bll.Levels.GetAllAsync(UserId),
                nameof(Level.Id),
                nameof(Level.Title));
            vm.TimesSelectList =
                new SelectList(await _bll.Times.GetAllAsync(UserId),
                    nameof(Time.Id), nameof(Time.From));
            vm.PackagesSelectList =
                new SelectList(await _bll.PackageGameTypeTimes.GetAllAsync(UserId),
                    nameof(PackageGameTypeTime.Id), nameof(PackageGameTypeTime.PackageGtName));
            return View(vm);
        }

        // GET: Contests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || !_bll.Contests.IsContestOwnedByUser(UserId, id.Value) )
            {
                return NotFound();
            }

            var contest = await _bll.Contests
                .FirstOrDefaultAsync(id.Value);
            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            //Remove contest gameTypes
            foreach (var gameType in _bll.ContestGameTypes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id)))
            {
                await _bll.ContestGameTypes.RemoveAsync(gameType);
            }
            
            //Remove contest levels
            foreach (var level in _bll.ContestLevels.GetAllAsync().Result.Where(e => e.ContestId.Equals(id)))
            {
                await _bll.ContestLevels.RemoveAsync(level);
            }
            
            //Remove contest package
            foreach (var package in _bll.ContestPackages.GetAllAsync().Result.Where(e => e.ContestId.Equals(id)))
            {
                await _bll.ContestPackages.RemoveAsync(package);
            }
            
            //Remove contest times
            foreach (var times in _bll.ContestTimes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id)))
            {
                await _bll.ContestTimes.RemoveAsync(times);
            }
            await _bll.Contests.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}