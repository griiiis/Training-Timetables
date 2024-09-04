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
    /// Contest Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimeOfDaysController : ControllerBase
    {
        /// <summary>
        /// Time of day constructor
        /// </summary>
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.TimeOfDay, TimeOfDay> _mapper;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Time of Day constructor
        /// </summary>
        /// <param name="userManager">User Manager</param>
        /// <param name="bll">Bll</param>
        /// <param name="autoMapper">AutoMapper</param>
        public TimeOfDaysController(UserManager<AppUser> userManager, IAppBLL bll, IMapper autoMapper)
        {
            _userManager = userManager;
            _bll = bll;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.TimeOfDay, TimeOfDay>(autoMapper);
        }

        /// <summary>
        /// Returns all time of days visible to current user
        /// </summary>
        /// <returns>List of time of days</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.TimeOfDay>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<App.DTO.v1_0.TimeOfDay>>> GetTimeOfDays()
        {
            var res = (await _bll.TimeOfDays.GetAllAsync(UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns contest time of days
        /// </summary>
        /// <returns>List of time of days</returns>
        [HttpGet("contest/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.TimeOfDay>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.TimeOfDay>>> GetContestTimeOfDays(Guid contestId)
        {
            var res = (await _bll.TimeOfDays.GetContestTimeOfDays(contestId));
            return Ok(res);
        }


        /// <summary>
        /// Returns TimeOfDay that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">TimeOfDay Id</param>
        /// <returns>TimeOfDay that matches given id and belongs to User</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.TimeOfDay>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.TimeOfDay>> GetTimeOfDay(Guid id)
        {
            if (!_bll.TimeOfDays.IsTimeOfDayOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var timeOfDay = await _bll.TimeOfDays.FirstOrDefaultAsync(id);
            if (timeOfDay == null)
            {
                return NotFound();
            }
            return Ok(timeOfDay);
        }

        /// <summary>
        /// Edit user's time of day
        /// </summary>
        /// <param name="id">TimeOfDay Id</param>
        /// <param name="timeOfDay">Time Of Day</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> PutTimeOfDay(Guid id, App.DTO.v1_0.TimeOfDay timeOfDay)
        {
            if (id != timeOfDay.Id)
            {
                return BadRequest();
            }
            _bll.TimeOfDays.UpdateTimeOfDayWithUser(UserId, _mapper.Map(timeOfDay)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.TimeOfDays.ExistsAsync(id))
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
        /// Adds new TimeOfDay 
        /// </summary>
        /// <param name="timeOfDay">Time Of Day</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.TimeOfDay>((int) HttpStatusCode.Created)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.TimeOfDay>> PostTimeOfDay(App.DTO.v1_0.TimeOfDay timeOfDay)
        {
            var newTimeOfDays = _bll.TimeOfDays.AddTimeOfDayWithUser(UserId, _mapper.Map(timeOfDay)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetTimeOfDay", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newTimeOfDays.Id
            }, _mapper.Map(newTimeOfDays));
        }

        /// <summary>
        /// Deletes TimeOfDay that matches id and belongs to User
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> DeleteTimeOfDay(Guid id)
        {
            if (!_bll.TimeOfDays.IsTimeOfDayOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var timeOfDay = await _bll.TimeOfDays.FirstOrDefaultAsync(id);
            if (timeOfDay == null)
            {
                return NotFound();
            }
            await _bll.TimeOfDays.RemoveAsync(timeOfDay);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
