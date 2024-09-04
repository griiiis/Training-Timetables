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
    /// GameType Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameTypesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.GameType, GameType> _mapper;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Game constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public GameTypesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.GameType, GameType>(autoMapper);
        }

        /// <summary>
        /// Returns all game types visible to current user
        /// </summary>
        /// <returns>List of game types</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.GameType>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<App.DTO.v1_0.GameType>>> GetGameTypes()
        {
            var res = (await _bll.GameTypes.GetAllAsync(UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns all contest's game types
        /// </summary>
        /// <param name="contestId">Contest Id</param>
        /// <returns>List of game types</returns>
        [HttpGet("{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.GameType>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.GameType>>> GetContestGameTypes(Guid contestId)
        {
            var res = (await _bll.GameTypes.GetAllCurrentContestAsync(contestId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns GameType that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">GameType Id</param>
        /// <returns>Game that matches given id and belongs to User</returns>
        [HttpGet("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.GameType>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.GameType>> GetGameType(Guid id)
        {
            if (!_bll.GameTypes.IsGameTypeOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var gameType = _mapper.Map(await _bll.GameTypes.FirstOrDefaultAsync(id));
            if (gameType == null)
            {
                return NotFound();
            }
            return Ok(gameType);
        }
        
        /// <summary>
        /// Returns GameType that matches given Id
        /// </summary>
        /// <param name="id">GameType Id</param>
        /// <returns>Game that matches given id</returns>
        [HttpGet("gameType/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.GameType>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.GameType>> GetGameTypeForAll(Guid id)
        {
            var gameType = _mapper.Map(await _bll.GameTypes.FirstOrDefaultAsync(id));
            if (gameType == null)
            {
                return NotFound();
            }
            return Ok(gameType);
        }

        /// <summary>
        /// Edit user's gametype
        /// </summary>
        /// <param name="id">GameType Id</param>
        /// <param name="gameType">GameType</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> PutGameType(Guid id, App.DTO.v1_0.GameType gameType)
        {
            if (id != gameType.Id)
            {
                return BadRequest();
            }
            _bll.GameTypes.UpdateGameTypeWithUser(UserId, _mapper.Map(gameType)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.GameTypes.ExistsAsync(id))
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
        /// Adds new GameType 
        /// </summary>
        /// <param name="gameType">GameType</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.GameType>((int) HttpStatusCode.Created)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.GameType>> PostGameType(App.DTO.v1_0.GameType gameType)
        {
            var newGameTypes = _bll.GameTypes.AddGameTypeWithUser(UserId, _mapper.Map(gameType)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetGameType", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newGameTypes.Id
            }, _mapper.Map(newGameTypes));
        }

        /// <summary>
        /// Deletes GameType that matches Id and belongs to User
        /// </summary>
        /// <param name="id">GameType Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> DeleteGameType(Guid id)
        {
            
            if (!_bll.GameTypes.IsGameTypeOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var gameType = await _bll.GameTypes.FirstOrDefaultAsync(id);
            if (gameType == null)
            {
                return NotFound();
            }

            await _bll.GameTypes.RemoveAsync(gameType);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
