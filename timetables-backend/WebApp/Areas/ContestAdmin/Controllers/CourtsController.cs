using App.Contracts.BLL;
using App.BLL.DTO;
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
    public class CourtsController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public CourtsController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }


        // GET: Courts
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Courts.GetAllAsync(UserId));
        }


        // GET: Courts/Create
        public IActionResult Create()
        {
            var vm = new CourtCreateEditViewModel()
            {
                GameTypeSelectList = new SelectList(_bll.GameTypes.GetAll(UserId),
                    nameof(GameType.Id),
                    nameof(GameType.GameTypeName)),
                LocationSelectList = new SelectList(_bll.Locations.GetAll(UserId),
                    nameof(Location.Id),
                    nameof(Location.LocationName))
            };
            return View(vm);
        }

        // POST: Courts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourtCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Courts.AddCourtWithUser(UserId, vm.Court);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.GameTypeSelectList = new SelectList(
                await _bll.GameTypes.GetAllAsync(UserId), nameof(GameType.Id),
                nameof(GameType.GameTypeName));
            vm.LocationSelectList = new SelectList(
                await _bll.Locations.GetAllAsync(UserId), nameof(Location.Id),
                nameof(Location.LocationName));
            return View(vm);
        }

// GET: Courts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !_bll.Courts.IsCourtOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var court = await _bll.Courts.FirstOrDefaultAsync(id.Value);
            if (court == null)
            {
                return NotFound();
            }

            var vm = new CourtCreateEditViewModel()
            {
                Court = court,
                GameTypeSelectList = new SelectList(
                    await _bll.GameTypes.GetAllAsync(UserId), nameof(GameType.Id),
                    nameof(GameType.GameTypeName)),
                LocationSelectList = new SelectList(
                    await _bll.Locations.GetAllAsync(UserId), nameof(Location.Id),
                    nameof(Location.LocationName))
            };
            return View(vm);
        }

// POST: Courts/Edit/5
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CourtCreateEditViewModel vm)
        {
            if (id != vm.Court.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.Courts.UpdateCourtWithUser(UserId, vm.Court);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.Courts.ExistsAsync(vm.Court.Id))
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

            vm.GameTypeSelectList = new SelectList(
                await _bll.GameTypes.GetAllAsync(UserId), nameof(GameType.Id),
                nameof(GameType.GameTypeName));
            vm.LocationSelectList = new SelectList(
                await _bll.Locations.GetAllAsync(UserId), nameof(Location.Id),
                nameof(Location.LocationName));
            return View(vm);
        }

        // GET: Courts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _bll.Courts
                .FirstOrDefaultAsync(id.Value);
            if (court == null)
            {
                return NotFound();
            }

            return View(court);
        }

        // POST: Courts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var court = await _bll.Courts
                .FirstOrDefaultAsync(id);
            if (court != null)
            {
                await _bll.Courts.RemoveAsync(court);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}