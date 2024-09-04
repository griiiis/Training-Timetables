using App.BLL.DTO;
using App.Contracts.BLL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.ContestAdmin.ViewModels;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class RolePreferencesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public RolePreferencesController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: RolePreferences/Create
        public async Task<IActionResult> Create(Guid contestId)
        {
            var gameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(contestId)).ToList();
            var levelSelectList = new SelectList(await _bll.Levels.GetAllCurrentContestAsync(contestId), nameof(Level.Id), nameof(Level.Title));
            var selectedLevelsList = new List<List<Guid>>(gameTypes.Count);
            var previousRolePreferences = (await _bll.RolePreferences.GetAllAsync(UserId)).ToList();
    
            for (int i = 0; i < gameTypes.Count; i++)
            {
                selectedLevelsList.Add(new List<Guid>());
            }

            var vm = new RolePreferenceCreateEditViewModel()
            {
                ContestId = contestId.ToString(),
                GameTypes = gameTypes,
                LevelSelectList = levelSelectList,
                SelectedLevelsList = selectedLevelsList, 
                PreviousRolePreferences = previousRolePreferences
            };
            return View(vm);
        }

        // POST: RolePreferences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolePreferenceCreateEditViewModel vm)
        {
            //Remove previous rolepreferences
            var rolePreferences = (await _bll.RolePreferences.GetAllAsync(UserId)).ToList();
            foreach (var role in rolePreferences)
            {
                await _bll.RolePreferences.RemoveAsync(role);
            }
            
            var gameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(Guid.Parse(vm.ContestId))).ToList();
            
            for(var i = 0; i < gameTypes.Count; i++)
            {
                foreach (var levelId in vm.SelectedLevelsList[i])
                {
                    if (levelId.ToString() == "-1")
                    {
                        break;
                    }
                    var rolePreference = new RolePreference
                    {
                        LevelId = levelId,
                        GameTypeId = gameTypes[i].Id,
                        ContestId = Guid.Parse(vm.ContestId)
                    };
                    _bll.RolePreferences.AddRolePreferenceWithUser(Guid.Parse(_userManager.GetUserId(User)!), rolePreference);
                }
            }
            await _bll.SaveChangesAsync(); 
            return RedirectToAction("MyContests", "Contests");
        }
    }
}
