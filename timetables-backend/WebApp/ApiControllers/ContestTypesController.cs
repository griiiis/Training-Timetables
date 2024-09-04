using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.BLL.DTO;
using App.Contracts.BLL;
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
    /// Contest Type Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Contest Admin")]
    public class ContestTypesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.ContestType, ContestType> _mapper;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Contest Type constructor
        /// </summary>
        public ContestTypesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.ContestType, ContestType>(autoMapper);
        }

        /// <summary>
        /// Returns all contest types visible to current user
        /// </summary>
        /// <returns>List of contest types</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.ContestType>>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.ContestType>>> GetContestTypes()
        {
            var res = (await _bll.ContestTypes.GetAllAsync(UserId))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns Contest Type that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Contest Type Id</param>
        /// <returns>Contest Type that matches given id and belongs to User</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.ContestType>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.ContestType>> GetContestType(Guid id)
        {
            if (!_bll.ContestTypes.IsContestTypeOwnedByUser(UserId, id))
            {
                return NotFound();
            }

            var contestType = _mapper.Map(await _bll.ContestTypes.FirstOrDefaultAsync(id));
            if (contestType == null)
            {
                return NotFound();
            }

            return Ok(contestType);
        }

        /// <summary>
        /// Edit user's contest type
        /// </summary>
        /// <param name="id">Contest Type Id</param>
        /// <param name="contestType">Contest Type</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutContestType(Guid id, App.DTO.v1_0.ContestType contestType)
        {
            if (id != contestType.Id && _bll.ContestTypes.IsContestTypeOwnedByUser(UserId, contestType.Id))
            {
                return BadRequest();
            }

            _bll.ContestTypes.UpdateContestTypeWithUser(UserId, _mapper.Map(contestType)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.ContestTypes.ExistsAsync(id))
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
        /// Adds Contest Type
        /// </summary>
        /// <param name="contestType">Contest Type</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.ContestType>((int)HttpStatusCode.Created)]
        public async Task<ActionResult<App.DTO.v1_0.ContestType>> PostContestType(App.DTO.v1_0.ContestType contestType)
        {
            var newContestType = _bll.ContestTypes.AddContestTypeWithUser(UserId, _mapper.Map(contestType)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetContestType", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newContestType.Id
            }, _mapper.Map(newContestType));
        }

        /// <summary>
        /// Deletes Contest Type that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Contest Type Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteContestType(Guid id)
        {
            if (!_bll.ContestTypes.IsContestTypeOwnedByUser(UserId, id))
            {
                return NotFound();
            }

            var contestType = await _bll.ContestTypes.FirstOrDefaultAsync(id);
            if (contestType == null)
            {
                return NotFound();
            }

            await _bll.ContestTypes.RemoveAsync(contestType);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}