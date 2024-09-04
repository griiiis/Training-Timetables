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
    public class GameTypesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public GameTypesController( UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: GameTypes
        public async Task<IActionResult> Index()
        {
            return View(await _bll.GameTypes.GetAllAsync(UserId));
        }

        // GET: GameTypes/Create
        public IActionResult Create()
        {
            return View(new GameType());
        }

        // POST: GameTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameType gameType)
        {
            if (ModelState.IsValid)
            {
                _bll.GameTypes.AddGameTypeWithUser(UserId, gameType);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameType);
        }

        // GET: GameTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !_bll.GameTypes.IsGameTypeOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var gameType = await _bll.GameTypes.FirstOrDefaultAsync(id.Value);
            if (gameType == null)
            {
                return NotFound();
            }
            return View(gameType);
        }

        // POST: GameTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, GameType gameType)
        {
            if (id != gameType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.GameTypes.UpdateGameTypeWithUser(UserId, gameType);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.GameTypes.ExistsAsync(gameType.Id))
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
            return View(gameType);
        }

        // GET: GameTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameType = await _bll.GameTypes
                .FirstOrDefaultAsync(id.Value);
            if (gameType == null)
            {
                return NotFound();
            }

            return View(gameType);
        }

        // POST: GameTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var gameType = await _bll.GameTypes.FirstOrDefaultAsync(id);
            if (gameType != null)
            {
                await _bll.GameTypes.RemoveAsync(gameType);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
