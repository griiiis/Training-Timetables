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
    /// PackageGameTypeTime Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PackageGameTypeTimesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.PackageGameTypeTime, PackageGameTypeTime> _mapper;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// PackageGameTypeTime constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public PackageGameTypeTimesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.PackageGameTypeTime, PackageGameTypeTime>(autoMapper);
        }

        /// <summary>
        /// Returns all packages visible to current user
        /// </summary>
        /// <returns>List of packages</returns>
        [HttpGet("owner")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.PackageGameTypeTime>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<App.DTO.v1_0.PackageGameTypeTime>>> GetPackageGameTypeTimes()
        {
            var res = (await _bll.PackageGameTypeTimes.GetAllAsync(UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns PackageGameTypeTime that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">PackageGameTypeTime Id</param>
        /// <returns>PackageGameTypeTime that matches given id and belongs to User</returns>
        [HttpGet("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.PackageGameTypeTime>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<App.DTO.v1_0.PackageGameTypeTime>> GetPackageGameTypeTime(Guid id)
        {
            if (!_bll.PackageGameTypeTimes.IsPackageGameTypeTimeOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var packageGameTypeTime = _mapper.Map(await _bll.PackageGameTypeTimes.FirstOrDefaultAsync(id));
            if (packageGameTypeTime == null)
            {
                return NotFound();
            }
            return Ok(packageGameTypeTime);
        }
        
        /// <summary>
        /// Returns PackageGameTypeTime that matches given Id
        /// </summary>
        /// <param name="id">PackageGameTypeTime Id</param>
        /// <returns>PackageGameTypeTime that matches given id</returns>
        [HttpGet("package/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.PackageGameTypeTime>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<App.DTO.v1_0.PackageGameTypeTime>> GetPackageGameTypeTimeForAll(Guid id)
        {
            var packageGameTypeTime = _mapper.Map(await _bll.PackageGameTypeTimes.FirstOrDefaultAsync(id));
            if (packageGameTypeTime == null)
            {
                return NotFound();
            }
            return Ok(packageGameTypeTime);
        }
        
        /// <summary>
        /// Returns all contest's packages
        /// </summary>
        /// <param name="contestId">Contest Id</param>
        /// <returns>List of packages</returns>
        [HttpGet("{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Time>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.Level>>> GetContestPackages(Guid contestId)
        {
            var res = (await _bll.PackageGameTypeTimes.GetAllCurrentContestAsync(contestId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">PackageGameTypeTime Id</param>
        /// <param name="packageGameTypeTime">PackageGameTypeTime</param>
        /// <returns></returns>
        [HttpPut("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> PutPackageGameTypeTime(Guid id,
            App.DTO.v1_0.PackageGameTypeTime packageGameTypeTime)
        {
            if (id != packageGameTypeTime.Id)
            {
                return BadRequest();
            }
            _bll.PackageGameTypeTimes.UpdatePackageGameTypeTimeWithUser(UserId, _mapper.Map(packageGameTypeTime)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.PackageGameTypeTimes.ExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Success");
        }

        /// <summary>
        /// Adds new packageGameTypeTime
        /// </summary>
        /// <param name="packageGameTypeTime">PackageGameTypeTime</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = "Contest Admin")]
        [ProducesResponseType<App.DTO.v1_0.PackageGameTypeTime>((int)HttpStatusCode.Created)]
        public async Task<ActionResult<App.DTO.v1_0.PackageGameTypeTime>> PostPackageGameTypeTime(App.DTO.v1_0.PackageGameTypeTime packageGameTypeTime)
        {
            var newPackage = _bll.PackageGameTypeTimes.AddPackageGameTypeTimeWithUser(UserId, _mapper.Map(packageGameTypeTime)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPackageGameTypeTime", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newPackage.Id
            }, _mapper.Map(newPackage));
        }

        /// <summary>
        /// Deletes PackageGameTypeTime that matches Id and belongs to User
        /// </summary>
        /// <param name="id">PackageGameTypeTime Id and belongs to User</param>
        /// <returns></returns>
        [HttpDelete("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> DeletePackageGameTypeTime(Guid id)
        {
            if (!_bll.PackageGameTypeTimes.IsPackageGameTypeTimeOwnedByUser(UserId, id))
            {
                return NotFound();
            }
            var packageGameTypeTime = await _bll.PackageGameTypeTimes.FirstOrDefaultAsync(id);
            if (packageGameTypeTime == null)
            {
                return NotFound();
            }
            await _bll.PackageGameTypeTimes.RemoveAsync(packageGameTypeTime);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}