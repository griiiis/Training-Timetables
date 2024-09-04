using App.Contracts.BLL;
using App.BLL.DTO;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.ContestAdmin.Controllers
{
    [Authorize(Roles = "Contest Admin")]
    [Area("ContestAdmin")]
    public class TimeOfDaysController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public TimeOfDaysController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: TimeOfDays
        public async Task<IActionResult> Index()
        {
            return View(await _bll.TimeOfDays.GetAllAsync(UserId));
        }

        // GET: TimeOfDays/Create
        public IActionResult Create()
        {
            return View(new TimeOfDay());
        }

        // POST: TimeOfDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeOfDay timeOfDay)
        {
            if (ModelState.IsValid)
            {
                _bll.TimeOfDays.AddTimeOfDayWithUser(UserId, timeOfDay);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeOfDay);
        }

        // GET: TimeOfDays/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !_bll.TimeOfDays.IsTimeOfDayOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var timeOfDay = await _bll.TimeOfDays.FirstOrDefaultAsync(id.Value);
            if (timeOfDay == null)
            {
                return NotFound();
            }
            return View(timeOfDay);
        }

        // POST: TimeOfDays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TimeOfDay timeOfDay)
        {
            if (id != timeOfDay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.TimeOfDays.UpdateTimeOfDayWithUser(UserId, timeOfDay);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.TimeOfDays.ExistsAsync(timeOfDay.Id))
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
            return View(timeOfDay);
        }

        // GET: TimeOfDays/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeOfDay = await _bll.TimeOfDays
                .FirstOrDefaultAsync(id.Value);
            if (timeOfDay == null)
            {
                return NotFound();
            }

            return View(timeOfDay);
        }

        // POST: TimeOfDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var timeOfDay = await _bll.TimeOfDays.FirstOrDefaultAsync(id);
            if (timeOfDay != null)
            {
                await _bll.TimeOfDays.RemoveAsync(timeOfDay);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
