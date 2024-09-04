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
    public class LevelsController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public LevelsController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: Levels
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Levels.GetAllAsync(UserId));
        }

        // GET: Levels/Create
        public IActionResult Create()
        {
            return View(new Level());
        }

        // POST: Levels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Level level)
        {
            if (ModelState.IsValid)
            {
                _bll.Levels.AddLevelWithUser(UserId, level);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(level);
        }

        // GET: Levels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !_bll.Levels.IsLevelOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var level = await _bll.Levels.FirstOrDefaultAsync(id.Value);
            if (level == null)
            {
                return NotFound();
            }
            return View(level);
        }

        // POST: Levels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Level level)
        {
            if (id != level.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.Levels.UpdateLevelWithUser(UserId, level);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _bll.Levels.ExistsAsync(level.Id))
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
            return View(level);
        }

        // GET: Levels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var level = await _bll.Levels
                .FirstOrDefaultAsync(id.Value);
            if (level == null)
            {
                return NotFound();
            }

            return View(level);
        }

        // POST: Levels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var level = await _bll.Levels.FirstOrDefaultAsync(id);
            if (level != null)
            {
                await _bll.Levels.RemoveAsync(level);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
