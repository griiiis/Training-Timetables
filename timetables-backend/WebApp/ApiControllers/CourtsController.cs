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
    /// Court Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Contest Admin")]
    public class CourtsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Court, Court> _mapper;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Court constructor
        /// </summary>
        public CourtsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Court, Court>(autoMapper);
        }

        /// <summary>
        /// Returns all courts visible to current user
        /// </summary>
        /// <returns>List of courts</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Court>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.Court>>> GetCourts()
        {
            var res = (await _bll.Courts.GetAllAsync(UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns Court that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Court Id</param>
        /// <returns>Court that matches given id and belongs to User</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Court>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.Court>> GetCourt(Guid id)
        {
            if (!_bll.Courts.IsCourtOwnedByUser(UserId, id))
            {
                return NotFound();
            }

            var court = _mapper.Map(await _bll.Courts.FirstOrDefaultAsync(id));
            if (court == null)
            {
                return NotFound();
            }

            return Ok(court);
        }

        /// <summary>
        /// Edit user's court
        /// </summary>
        /// <param name="id">Court Id</param>
        /// <param name="court">Court</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutCourt(Guid id, App.DTO.v1_0.Court court)
        {
            if (id != court.Id)
            {
                return BadRequest();
            }

            _bll.Courts.UpdateCourtWithUser(UserId, _mapper.Map(court)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Courts.ExistsAsync(id))
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
        /// Adds Court
        /// </summary>
        /// <param name="court">Court</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Court>((int)HttpStatusCode.Created)]
        public async Task<ActionResult<App.DTO.v1_0.Court>> PostCourt(App.DTO.v1_0.Court court)
        {
            var newCourt = _bll.Courts.AddCourtWithUser(UserId, _mapper.Map(court)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCourt", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newCourt.Id
            }, _mapper.Map(newCourt));
        }

        /// <summary>
        /// Deletes Court that matches Id and belongs to User
        /// </summary>
        /// <param name="id">Court Id</param>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCourt(Guid id, Guid userId)
        {
            if (!_bll.Courts.IsCourtOwnedByUser(userId, id))
            {
                return NotFound();
            }

            var court = await _bll.Courts.FirstOrDefaultAsync(id);
            if (court == null)
            {
                return NotFound();
            }

            await _bll.Courts.RemoveAsync(court);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}