using App.BLL.DTO;
using App.Contracts.BLL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.ContestAdmin.Controllers
{
    [Authorize(Roles = "Contest Admin")]
    [Area("ContestAdmin")]
    public class ContestUserRoleController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public ContestUserRoleController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;;
        }

        // GET: ContestUserRole
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ContestUserRoles.GetAllAsync(default));
        }

        // GET: ContestUserRole/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AppUserId"] = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "FirstName");
            ViewData["ContestRoleId"] = new SelectList(await _bll.ContestRoles.GetAllAsync(), "Id", "ContestRoleName");
            return View();
        }

        // POST: ContestUserRole/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContestUserRole contestUserRole)
        {
            if (ModelState.IsValid)
            {
                _bll.ContestUserRoles.Add(contestUserRole);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "FirstName", contestUserRole.AppUser!.Id);
            ViewData["ContestRoleId"] = new SelectList(await _bll.ContestRoles.GetAllAsync(), "Id", "ContestRoleName", contestUserRole.ContestRoleId);
            return View(contestUserRole);
        }

        // GET: ContestUserRole/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contestUserRole = await _bll.ContestUserRoles.FirstOrDefaultAsync(id.Value);
            if (contestUserRole == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "FirstName", contestUserRole.AppUser!.Id);
            ViewData["ContestRoleId"] = new SelectList(await _bll.ContestRoles.GetAllAsync(), "Id", "ContestRoleName", contestUserRole.ContestRoleId);
            return View(contestUserRole);
        }

        // POST: ContestUserRole/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ContestUserRole contestUserRole)
        {
            if (id != contestUserRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.ContestUserRoles.Update(contestUserRole);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.ContestUserRoles.ExistsAsync(contestUserRole.Id))
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
            ViewData["AppUserId"] = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "FirstName", contestUserRole.AppUser!.Id);
            ViewData["ContestRoleId"] = new SelectList(await _bll.ContestRoles.GetAllAsync(), "Id", "ContestRoleName", contestUserRole.ContestRoleId);
            return View(contestUserRole);
        }

        // GET: ContestUserRole/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contestUserRole = await _bll.ContestUserRoles.FirstOrDefaultAsync(id.Value);
            if (contestUserRole == null)
            {
                return NotFound();
            }

            return View(contestUserRole);
        }

        // POST: ContestUserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contestUserRole = await _bll.ContestUserRoles.FirstOrDefaultAsync(id);
            if (contestUserRole != null)
            {
                await _bll.ContestUserRoles.RemoveAsync(contestUserRole);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
