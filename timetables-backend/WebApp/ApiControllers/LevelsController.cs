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
    /// Level Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LevelsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Level, Level> _mapper;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Level constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public LevelsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Level, Level>(autoMapper);
        }

        /// <summary>
        /// Returns all levels visible to current user
        /// </summary>
        /// <returns>List of levels</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Level>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<App.DTO.v1_0.Level>>> GetLevels()
        {
            var res = (await _bll.Levels.GetAllAsync(UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns all contest's levels
        /// </summary>
        /// <param name="contestId">Contest Id</param>
        /// <returns>List of levels</returns>
        [HttpGet("{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Time>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.Level>>> GetContestLevels(Guid contestId)
        {
            var res = (await _bll.Levels.GetAllCurrentContestAsync(contestId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns Level that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Level Id</param>
        /// <returns>Level that matches given id and belongs to User</returns>
        [HttpGet("level/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Level>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.Level>> GetLevelForAll(Guid id)
        {
            var level = _mapper.Map(await _bll.Levels.FirstOrDefaultAsync(id));
            if (level == null)
            {
                return NotFound();
            }
            return Ok(level);
        }

        /// <summary>
        /// Returns Level that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Level Id</param>
        /// <returns>Level that matches given id and belongs to User</returns>
        [HttpGet("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Level>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.Level>> GetLevel(Guid id)
        {
            if (!_bll.Levels.IsLevelOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var level = _mapper.Map(await _bll.Levels.FirstOrDefaultAsync(id));
            if (level == null)
            {
                return NotFound();
            }
            return Ok(level);
        }

        /// <summary>
        /// Edit user's level
        /// </summary>
        /// <param name="id">Level Id</param>
        /// <param name="level">Level</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> PutLevel(Guid id, App.DTO.v1_0.Level level)
        {
            if (id != level.Id)
            {
                return BadRequest();
            }
            _bll.Levels.UpdateLevelWithUser(UserId, _mapper.Map(level)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Levels.ExistsAsync(id))
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
        /// Adds new Level 
        /// </summary>
        /// <param name="level">Level</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Level>((int) HttpStatusCode.Created)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.Level>> PostLevel(App.DTO.v1_0.Level level)
        {
            var newLevel = _bll.Levels.AddLevelWithUser(UserId, _mapper.Map(level)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetLevel", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newLevel.Id
            }, _mapper.Map(newLevel));
        }

        /// <summary>
        /// Deletes Level that matches Id and belongs to User
        /// </summary>
        /// <param name="id">Level Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> DeleteLevel(Guid id)
        {
            if (!_bll.Levels.IsLevelOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var level = await _bll.Levels.FirstOrDefaultAsync(id);
            if (level == null)
            {
                return NotFound();
            }
            await _bll.Levels.RemoveAsync(level);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
