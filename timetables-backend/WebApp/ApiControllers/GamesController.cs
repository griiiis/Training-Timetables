using System.Net;
using App.BLL.DTO;
using App.BLL.DTO.Models;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
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
    /// Games Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GamesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Game, Game> _mapper;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.CreateGamesData, CreateGamesData> _mapperData;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Game constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public GamesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Game, Game>(autoMapper);
            _mapperData = new PublicDTOBllMapper<App.DTO.v1_0.CreateGamesData, CreateGamesData>(autoMapper);
        }
        
        /// <summary>
        /// Returns all contest games
        /// </summary>
        /// <returns>List of games</returns>
        [HttpGet("contestGames/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Game>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<App.DTO.v1_0.Game>>> GetContestGames(Guid contestId)
        {
            var res = (await _bll.Games.GetContestGames(contestId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns all user current contest games
        /// </summary>
        /// <returns>List of games</returns>
        [HttpGet("userGames/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Game>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.Game>>> GetUserContestGames(Guid contestId)
        {
            var res = (await _bll.Games.GetUserContestGames(contestId, UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns Game that matches given Id
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <returns>Game that matches given id</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Game>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.Game>> GetGame(Guid id)
        {
            var game = _mapper.Map(await _bll.Games.FirstOrDefaultAsync(id));

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }
        
        /// <summary>
        /// Returns boolean if contest has any games
        /// </summary>
        /// <param name="contestId">Contest Id</param>
        /// <returns>Boolean</returns>
        [HttpGet("anyGames/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<bool>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public ActionResult<bool> AnyContestGames(Guid contestId)
        {
            return Ok(_bll.Games.AnyContestGames(contestId));
        }
        

        /// <summary>
        /// Creates new Games that belong to contest
        /// </summary>
        /// <param name="gamesData">All selected lists</param>
        /// <param name="contestId">Contest Id</param>
        /// <returns></returns>
        [HttpPost("{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.CreateGamesData>((int) HttpStatusCode.Created)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.CreateGamesData>> PostGame(App.DTO.v1_0.CreateGamesData gamesData, Guid contestId)
        {
            _bll.Games.CreateGames(_mapperData.Map(gamesData), contestId);
            await _bll.SaveChangesAsync();
            return CreatedAtAction("GetGame", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
            });
        }

        /// <summary>
        /// Deletes Game that matches id
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var game = await _bll.Games.FirstOrDefaultAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            await _bll.Games.RemoveAsync(game);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
