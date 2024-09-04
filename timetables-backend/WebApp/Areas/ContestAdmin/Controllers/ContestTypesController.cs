using App.BLL.DTO;
using App.Contracts.BLL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.ContestAdmin.Controllers
{
    [Authorize(Roles = "Contest Admin")]
    [Area("ContestAdmin")]
    public class ContestTypesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppBLL _bll;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);
        
        public ContestTypesController( UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: ContestTypes
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ContestTypes.GetAllAsync(UserId));
        }

        // GET: ContestTypes/Create
        public IActionResult Create()
        {
            return View(new ContestType());
        }

        // POST: ContestTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContestType contestType)
        {
            if (ModelState.IsValid)
            {
                _bll.ContestTypes.AddContestTypeWithUser(UserId, contestType);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contestType);
        }

        // GET: ContestTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !_bll.ContestTypes.IsContestTypeOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var contestType = await _bll.ContestTypes.FirstOrDefaultAsync(id.Value);
            
            if (contestType == null)
            {
                return NotFound();
            }
            return View(contestType);
        }

        // POST: ContestTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ContestType contestType)
        {
            if (id != contestType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.ContestTypes.UpdateContestTypeWithUser(UserId, contestType);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _bll.ContestTypes.ExistsAsync(contestType.Id))
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
            return View(contestType);
        }

        // GET: ContestTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || !_bll.ContestTypes.IsContestTypeOwnedByUser(UserId, id.Value))
            {
                return NotFound();
            }

            var contestType = await _bll.ContestTypes.FirstOrDefaultAsync(id.Value);
            if (contestType == null)
            {
                return NotFound();
            }

            return View(contestType);
        }

        // POST: ContestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contestType = await _bll.ContestTypes.FirstOrDefaultAsync(id);
            if (contestType != null)
            {
                await _bll.ContestTypes.RemoveAsync(contestType);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
