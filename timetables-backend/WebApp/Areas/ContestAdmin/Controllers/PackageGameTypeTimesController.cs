using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.DTO;
using App.Domain.Identity;
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
    public class PackageGameTypeTimesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public PackageGameTypeTimesController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }
    
        // GET: PackageGameTypeTimes
        public async Task<IActionResult> Index()
        {
            return View(await _bll.PackageGameTypeTimes.GetAllAsync(UserId));
        }

        // GET: PackageGameTypeTimes/Create
        public IActionResult Create()
        {
            var vm = new PackageGameTypeTimeCreateEditViewModel()
            {
                GameTypeSelectList = new SelectList(_bll.GameTypes.GetAll(UserId), nameof(GameType.Id),
                    nameof(GameType.GameTypeName))
            };
            return View(vm);
        }

        // POST: PackageGameTypeTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PackageGameTypeTimeCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.PackageGameTypeTimes.AddPackageGameTypeTimeWithUser(UserId, vm.PackageGameTypeTime);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.GameTypeSelectList = new SelectList(await _bll.GameTypes.GetAllAsync(UserId), nameof(GameType.Id),
                nameof(GameType.GameTypeName));
            return View(vm);
        }

        // GET: PackageGameTypeTimes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !_bll.PackageGameTypeTimes.IsPackageGameTypeTimeOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var packageGameTypeTime = await _bll.PackageGameTypeTimes
                .FirstOrDefaultAsync(id.Value);
            
            if (packageGameTypeTime == null)
            {
                return NotFound();
            }

            var vm = new PackageGameTypeTimeCreateEditViewModel()
            {
                PackageGameTypeTime = packageGameTypeTime,
                GameTypeSelectList = new SelectList(await _bll.GameTypes.GetAllAsync(UserId), nameof(GameType.Id),
                    nameof(GameType.GameTypeName))
            };
            return View(vm);
        }

        // POST: PackageGameTypeTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PackageGameTypeTimeCreateEditViewModel vm)
        {
            if (id != vm.PackageGameTypeTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.PackageGameTypeTimes.Update(vm.PackageGameTypeTime);
                    
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.PackageGameTypeTimes.ExistsAsync(vm.PackageGameTypeTime.Id))
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
            
            vm.GameTypeSelectList = new SelectList(await _bll.GameTypes.GetAllAsync(UserId), nameof(GameType.Id),
                nameof(GameType.GameTypeName));
            return View(vm);
        }

        // GET: PackageGameTypeTimes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageGameTypeTime = await _bll.PackageGameTypeTimes
                .FirstOrDefaultAsync(id.Value);
            if (packageGameTypeTime == null)
            {
                return NotFound();
            }

            return View(packageGameTypeTime);
        }

        // POST: PackageGameTypeTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var packageGameTypeTime = await _bll.PackageGameTypeTimes.FirstOrDefaultAsync(id);
            if (packageGameTypeTime != null)
            {
                await _bll.PackageGameTypeTimes.RemoveAsync(packageGameTypeTime);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}