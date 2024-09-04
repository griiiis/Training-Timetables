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
    /// Location Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Contest Admin")]
    public class LocationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Location, Location> _mapper;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);
        
        /// <summary>
        /// Location constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public LocationsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Location, Location>(autoMapper);
        }
        
        /// <summary>
        /// Returns all locations visible to current user
        /// </summary>
        /// <returns>List of locations</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Location>>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.Location>>> GetLocations()
        {
            var res = (await _bll.Locations.GetAllAsync(UserId))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns Location that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <returns>Location that matches given id and belongs to User</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Location>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.Location>> GetLocation(Guid id)
        {
            if (!_bll.Locations.IsLocationOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var location = _mapper.Map(await _bll.Locations.FirstOrDefaultAsync(id));

            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        /// <summary>
        /// Edit user's location
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <param name="location">Location</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutLocation(Guid id, App.DTO.v1_0.Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }
            _bll.Locations.UpdateLocationWithUser(UserId, _mapper.Map(location)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Locations.ExistsAsync(id))
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
        /// Adds new Location
        /// </summary>
        /// <param name="location">Location</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Location>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<App.DTO.v1_0.Location>> PostLocation(App.DTO.v1_0.Location location)
        {
            var newLocation = _bll.Locations.AddLocationWithUser(UserId, _mapper.Map(location)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newLocation.Id
            }, _mapper.Map(newLocation));
        }

        /// <summary>
        /// Deletes Location that matches Id and belongs to User
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            if (!_bll.Locations.IsLocationOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var location = await _bll.Locations.FirstOrDefaultAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            await _bll.Locations.RemoveAsync(location);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
