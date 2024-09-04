using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using App.DTO.v1_0.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;
using AppUser = App.Domain.Identity.AppUser;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// App User Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Contest Admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppUserController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Identity.AppUser, App.BLL.DTO.Identity.AppUser> _mapper;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.UserContestPackage, App.BLL.DTO.UserContestPackage> _userContestPackageMapper;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// AppUser constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public AppUserController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper, RoleManager<AppRole> roleManager)
        {
            _bll = bll;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Identity.AppUser, App.BLL.DTO.Identity.AppUser>(autoMapper);
            _userContestPackageMapper = new PublicDTOBllMapper<App.DTO.v1_0.UserContestPackage, App.BLL.DTO.UserContestPackage>(autoMapper);
        }
        
        /// <summary>
        /// Returns user
        /// </summary>
        /// <returns>User</returns>
        [HttpGet("{userId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Identity.AppUserModel>>((int)HttpStatusCode.OK)]
        public async Task<ActionResult<App.DTO.v1_0.Identity.AppUserModel>> GetUser(Guid userId)
        {
            var appUser = await _bll.AppUsers.FirstOrDefaultAsync(userId);
            if (appUser == null)
            {
                return NotFound();
            }

            var roles = _roleManager.Roles.ToList();

            var user = await _userManager.FindByIdAsync(appUser.Id.ToString());

            var roleName = (await _userManager.GetRolesAsync(user)).First();
            
            var userRoles = await _roleManager.FindByNameAsync(roleName);
            
            var vm = new AppUserModel
            {
                AppUser = _mapper.Map(appUser)!,
                SelectedRoleId = userRoles!.Id,
                RoleSelectList = roles
            };
            return vm;
        }
        
        /// <summary>
        /// Returns contest Users
        /// </summary>
        /// <returns>User</returns>
        [HttpGet("users/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Identity.AppUser>>((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<App.DTO.v1_0.Identity.AppUser>> GetContestUsersToAddToTeam(Guid contestId)
        {
            return (await _bll.AppUsers.GetAllContestUsers(contestId)).Select(e => _mapper.Map(e));
        }


        /// <summary>
        /// Edit user's contest
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="vm">App User Edit ViewModel</param>
        /// <returns></returns>
        [HttpPut("{userId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAppUser(Guid userId, AppUserModel vm)
        {
            if (userId != vm.AppUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(userId.ToString());

                    await _userManager.RemoveFromRolesAsync(user,
                        (await _userManager.GetRolesAsync(user)).ToList());

                    await _userManager.AddToRoleAsync(user,
                        _roleManager.Roles.First(e => e.Id == vm.SelectedRoleId).Name!);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.AppUsers.ExistsAsync(userId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok("Success");
        }
    }
}