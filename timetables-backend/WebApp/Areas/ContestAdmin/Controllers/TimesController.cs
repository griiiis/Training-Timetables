using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.ContestAdmin.ViewModels;
using TimeOfDay = App.Domain.TimeOfDay;

namespace WebApp.Areas.ContestAdmin.Controllers
{
    [Authorize(Roles = "Contest Admin")]
    [Area("ContestAdmin")]
    public class TimesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public TimesController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: Times
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Times.GetAllAsync(UserId));
        }

        // GET: Times/Create
        public IActionResult Create()
        {
            var vm = new TimeCreateEditViewModel()
            {
                TimeOfDaySelectList = new SelectList(_bll.TimeOfDays.GetAll(UserId), nameof(TimeOfDay.Id),
                    nameof(TimeOfDay.TimeOfDayName))
            };
            return View(vm);
        }

        // POST: Times/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Times.AddTimeWithUser(UserId, vm.Time);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.TimeOfDaySelectList = new SelectList(await _bll.TimeOfDays.GetAllAsync(UserId), nameof(TimeOfDay.Id),
                nameof(TimeOfDay.TimeOfDayName));
            return View(vm);
        }

        // GET: Times/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !_bll.Times.IsTimeOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var time = await _bll.Times.FirstOrDefaultAsync(id.Value);
            if (time == null)
            {
                return NotFound();
            }
            var vm = new TimeCreateEditViewModel()
            {
                Time = time,
                TimeOfDaySelectList = new SelectList(await _bll.TimeOfDays.GetAllAsync(UserId), nameof(TimeOfDay.Id),
                    nameof(TimeOfDay.TimeOfDayName))
            };
            return View(vm);
        }

        // POST: Times/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TimeCreateEditViewModel vm)
        {
            if (id != vm.Time.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.Times.UpdateTimeWithUser(UserId, vm.Time);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _bll.Times.ExistsAsync(vm.Time.Id))
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
            vm.TimeOfDaySelectList = new SelectList(await _bll.TimeOfDays.GetAllAsync(UserId), nameof(TimeOfDay.Id),
                nameof(TimeOfDay.TimeOfDayName));
            return View(vm);
        }

        // GET: Times/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _bll.Times
                .FirstOrDefaultAsync(id.Value);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var time = await _bll.Times.FirstOrDefaultAsync(id);
            if (time != null)
            {
                await _bll.Times.RemoveAsync(time);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
