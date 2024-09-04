using System.Net;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RolePreference Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolePreferencesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.RolePreference, RolePreference> _mapper;

        public RolePreferencesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.RolePreference, RolePreference>(autoMapper);
        }

        /// <summary>
        /// Returns all rolePreferences visible to current user
        /// </summary>
        /// <returns>List of rolePreferences</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.RolePreference>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.RolePreference>>> GetRolePreferences()
        {
            var res = (await _bll.RolePreferences.GetAllAsync(Guid.Parse(_userManager.GetUserId(User)!))).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Add Role Preferences to user
        /// </summary>
        /// <returns>List of rolePreferences</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.RolePreference>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.RolePreferenceViewModel>>> AddRolePreferences(App.DTO.v1_0.RolePreferenceViewModel vm)
        {
            //Remove previous rolepreferences
            var rolePreferences = (await _bll.RolePreferences.GetAllAsync(Guid.Parse(_userManager.GetUserId(User)!))).ToList();
            foreach (var role in rolePreferences)
            {
                await _bll.RolePreferences.RemoveAsync(role);
            }
            
            var gameTypes = (await _bll.GameTypes.GetAllCurrentContestAsync(Guid.Parse(vm.ContestId))).ToList();
            
            for(var i = 0; i < gameTypes.Count; i++)
            {
                foreach (var levelId in vm.SelectedLevelsList[i])
                {
                    if (levelId == "-1")
                    {
                        break;
                    }
                    if (levelId == "")
                    {
                        continue;
                    }
                    var rolePreference = new RolePreference
                    {
                        LevelId = Guid.Parse(levelId),
                        GameTypeId = gameTypes[i].Id,
                        ContestId = Guid.Parse(vm.ContestId)
                    };
                    _bll.RolePreferences.AddRolePreferenceWithUser(Guid.Parse(_userManager.GetUserId(User)!), rolePreference);
                }
            }
            await _bll.SaveChangesAsync(); 
            return Ok("Success!");
        }
        
        
        

        /// <summary>
        /// Returns RolePreference that matches given Id
        /// </summary>
        /// <param name="id">RolePreference Id</param>
        /// <returns>RolePreference that matches given Id</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.RolePreference>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.RolePreference>> GetRolePreference(Guid id)
        {
            var rolePreference = _mapper.Map(await _bll.RolePreferences.FirstOrDefaultAsync(id));

            if (rolePreference == null)
            {
                return NotFound();
            }

            return Ok(rolePreference);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">RolePreference Id</param>
        /// <param name="rolePreference">RolePreference</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutRolePreference(Guid id, App.DTO.v1_0.RolePreference rolePreference)
        {
            if (id != rolePreference.Id)
            {
                return BadRequest();
            }

            _bll.RolePreferences.Update(_mapper.Map(rolePreference));

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.RolePreferences.ExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Success!");
        }

        /// <summary>
        /// Adds new rolePreference 
        /// </summary>
        /// <param name="rolePreference">RolePreference</param>
        /// <returns></returns>
        [HttpDelete]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.RolePreference>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<App.DTO.v1_0.RolePreference>> PostRolePreference(App.DTO.v1_0.RolePreference rolePreference)
        {
            _bll.RolePreferences.Add(_mapper.Map(rolePreference));
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetRolePreference", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = rolePreference.Id
            }, rolePreference);
        }

        /// <summary>
        /// Deletes RolePreference that matches Id
        /// </summary>
        /// <param name="id">RolePreference Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteRolePreference(Guid id)
        {
            var rolePreference = await _bll.RolePreferences.FirstOrDefaultAsync(id);
            if (rolePreference == null)
            {
                return NotFound();
            }

            await _bll.RolePreferences.RemoveAsync(rolePreference);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
