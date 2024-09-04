using App.BLL.DTO;
using App.Contracts.BLL;
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
    public class AppUserController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AppUserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IAppBLL bll)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _bll = bll;
        }

        // GET: AppUser
        public async Task<IActionResult> Index(Guid contestId)
        {
            var packages = await _bll.UserContestPackages.GetContestUsers(contestId);
            var contestRoles = await _bll.ContestUserRoles.GetAllAsync(default);
            
            var vm = new AppUserIndexViewModel
            {
                UserRoleModels = packages.Select(package => 
                    new UserRoleModel { Package = package, 
                        Role = contestRoles.Where(e => e.ContestRole!.ContestId.Equals(contestId) 
                                                       && e.AppUserId.Equals(package.AppUserId))
                            .Select(e => e.ContestRole)
                            .FirstOrDefault()! })
                    .ToList(),
                ContestId = contestId
            };
            return View(vm);
        }
        
        // GET: AppUser/Edit/5
        public async Task<IActionResult> Edit(Guid userId, Guid contestId)
        {
            var appUser = await _bll.AppUsers.FirstOrDefaultAsync(userId);
            if (appUser == null)
            {
                return NotFound();
            }

            var userRole = await _bll.ContestUserRoles.GetContestUserRole(userId, contestId);
            Console.WriteLine("SIIIIIIIIIN");
            Console.WriteLine(userRole.ContestRoleId);
            var contestRoles = await _bll.ContestRoles.ContestRoles(contestId);
            
            var vm = new AppUserEditViewModel
            {
                AppUser = appUser,
                SelectedRoleId = userRole.ContestRoleId,
                RoleSelectList = new SelectList(contestRoles ,nameof(ContestRole.Id),nameof(ContestRole.ContestRoleName)),
                ContestId = contestId,
            };
            return View(vm);
        }

        // POST: AppUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid contestId, AppUserEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var contestUserRole = await _bll.ContestUserRoles.GetContestUserRole(vm.AppUser.Id, vm.ContestId);
                    await _bll.ContestUserRoles.RemoveAsync(contestUserRole);

                    var newUserRole = new ContestUserRole
                    {
                        ContestRoleId = vm.SelectedRoleId,
                        AppUserId = vm.AppUser.Id
                    };

                    _bll.ContestUserRoles.Add(newUserRole);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.AppUsers.ExistsAsync(vm.AppUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "AppUser", new { contestId = contestId });
            }
            return View(vm);
        }
    }
}
