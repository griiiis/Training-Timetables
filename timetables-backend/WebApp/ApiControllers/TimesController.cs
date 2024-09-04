using System.Net;
using App.BLL.DTO;
using App.Contracts.BLL;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Helpers;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// Time Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Time, Time> _mapper;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);
        
        /// <summary>
        /// Time constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public TimesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Time, Time>(autoMapper);
        }

        /// <summary>
        /// Returns all times visible to current user
        /// </summary>
        /// <returns>List of times</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Time>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<App.DTO.v1_0.Time>>> GetTimes()
        {
            var res = (await _bll.Times.GetAllAsync(UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns all contest's times
        /// </summary>
        /// <param name="contestId">Contest Id</param>
        /// <returns>List of times</returns>
        [HttpGet("{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Time>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.Time>>> GetContestTimes(Guid contestId)
        {
            var res = (await _bll.Times.GetAllCurrentContestAsync(contestId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns Time that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Time Id</param>
        /// <returns>Time that matches given id and belongs to User</returns>
        [HttpGet("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Time>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.Time>> GetTime(Guid id)
        {
            if (!_bll.Times.IsTimeOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var time = _mapper.Map(await _bll.Times.FirstOrDefaultAsync(id));
            if (time == null)
            {
                return NotFound();
            }
            return Ok(time);
        }

        /// <summary>
        /// Edit user's time
        /// </summary>
        /// <param name="id">Time Id</param>
        /// <param name="time">Time</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> PutTime(Guid id, App.DTO.v1_0.Time time)
        {
            if (id != time.Id)
            {
                return BadRequest();
            }
            _bll.Times.UpdateTimeWithUser(UserId, _mapper.Map(time)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Times.ExistsAsync(id))
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
        /// Adds new Time 
        /// </summary>
        /// <param name="time">Time</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Time>((int) HttpStatusCode.Created)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.Time>> PostTime(App.DTO.v1_0.Time time)
        {
            var newTime = _bll.Times.AddTimeWithUser(UserId, _mapper.Map(time)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetTime", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newTime.Id
            }, _mapper.Map(newTime));
        }

        /// <summary>
        /// Deletes Time that matches Id and belongs to User
        /// </summary>
        /// <param name="id">Time Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> DeleteTime(Guid id)
        {
            if (!_bll.Times.IsTimeOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var time = await _bll.Times.FirstOrDefaultAsync(id);
            if (time == null)
            {
                return NotFound();
            }
            await _bll.Times.RemoveAsync(time);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
