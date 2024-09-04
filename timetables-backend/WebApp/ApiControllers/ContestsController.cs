using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using App.DTO.v1_0;
using App.DTO.v1_0.Models.Contests;
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
    public class ContestsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<Contest, App.BLL.DTO.Contest> _mapper;
        private readonly PublicDTOBllMapper<ContestEditModel, App.BLL.DTO.Models.Contests.ContestEditModel> _editModelMapper;
        private readonly PublicDTOBllMapper<ContestCreateModel, App.BLL.DTO.Models.Contests.ContestCreateModel> _createModelMapper;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Contest constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="autoMapper">AutoMapper</param>
        public ContestsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<Contest, App.BLL.DTO.Contest>(autoMapper);
            _editModelMapper = new PublicDTOBllMapper<ContestEditModel, App.BLL.DTO.Models.Contests.ContestEditModel>(autoMapper);
            _createModelMapper = new PublicDTOBllMapper<ContestCreateModel, App.BLL.DTO.Models.Contests.ContestCreateModel>(autoMapper);
        }

        /// <summary>
        /// Returns all contests visible to all users
        /// </summary>
        /// <returns>List of contests</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<Contest>>((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Contest>>> GetContests()
        {
            var res = (await _bll.Contests.GetAllAsync(default))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns all contests visible to current owner
        /// </summary>
        /// <returns>List of contests</returns>
        [HttpGet("owner")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<Contest>>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<Contest>>> GetOwnerContests()
        {
                var res = (await _bll.Contests.GetAllAsync(UserId))
                    .Select(e => _mapper.Map(e)).ToList();
                return Ok(res);
        }

        /// <summary>
        /// Returns all contests visible to current user
        /// </summary>
        /// <returns>List of contests</returns>
        [HttpGet("user")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<Contest>>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Contest>>> GetUserContests()
        {
            var res = (await _bll.Contests.GetUserContests(UserId))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns Contest that matches given Id
        /// </summary>
        /// <param name="id">Contest Id</param>
        /// <returns>Contest that matches given Id</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<Contest>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Contest>> GetContest(Guid id)
        {
            var contest = _mapper.Map(await _bll.Contests.FirstOrDefaultAsync(id));

            if (contest == null)
            {
                return NotFound();
            }

            return Ok(contest);
        }

        /// <summary>
        /// Returns Contest that matches given Id and belongs to User
        /// </summary>
        /// <param name="id">Contest Id</param>
        /// <returns>Contest that matches given Id</returns>
        [HttpGet("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<Contest>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<Contest>> GetContestById(Guid id)
        {
            if (!_bll.Contests.IsContestOwnedByUser(UserId, id))
            {
                return NotFound();
            }

            var contest = _mapper.Map(await _bll.Contests.FirstOrDefaultAsync(id));

            if (contest == null)
            {
                return NotFound();
            }

            return Ok(contest);
        }

        [HttpGet("owner/edit/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<Contest>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<ContestEditModel>> EditContest(Guid id)
        {
            if (!_bll.Contests.IsContestOwnedByUser(UserId, id))
            {
                return NotFound();
            }

            var contest = _mapper.Map(await _bll.Contests.FirstOrDefaultAsync(id));

            if (contest == null)
            {
                return NotFound();
            }

            var vm = _editModelMapper.Map(await _bll.Contests.GetContestEditModel(UserId, id));
            
            return Ok(vm);
        }

        /// <summary>
        /// Edit user's contest
        /// </summary>
        /// <param name="id">Contest Id</param>
        /// <param name="contest">Contest</param>
        /// <returns></returns>
        [HttpPut("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> PutContest(Guid id, ContestEditModel contest)
        {
            
            if (id != contest.Contest.Id)
            {
                return BadRequest();
            }

            try
            {
                await _bll.Contests.PutContest(UserId, id, _editModelMapper.Map(contest));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Contests.ExistsAsync(id))
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
        /// Adds new Contest with User
        /// </summary>
        /// <param name="contest">Contest</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<Contest>((int)HttpStatusCode.Created)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<Contest>> PostContest(ContestCreateModel contest)
        {
            if (contest.SelectedLevelIds!.Count < 1 || contest.SelectedTimesIds!.Count < 1 ||
                contest.SelectedPackagesIds!.Count < 1)
            {
                return NotFound("Required forms not filled!");
            }
            
            var newContest = await _bll.Contests.PostContest(UserId, _createModelMapper.Map(contest));

            return CreatedAtAction("GetContest", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newContest.Id
            }, _mapper.Map(newContest));
        }

        /// <summary>
        /// Deletes Contest that matches Id and belongs to User
        /// </summary>
        /// <param name="id">Contest Id</param>
        /// <returns></returns>
        [HttpDelete("owner/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> DeleteContest(Guid id)
        {
            if (!_bll.Contests.IsContestOwnedByUser(UserId, id))
            {
                return NotFound();
            }

            var contest = await _bll.Contests.FirstOrDefaultAsync(id);
            if (contest == null)
            {
                return NotFound();
            }

            //Remove contest gameTypes
            foreach (var gameType in _bll.ContestGameTypes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id)))
            {
                await _bll.ContestGameTypes.RemoveAsync(gameType);
            }

            //Remove contest levels
            foreach (var level in _bll.ContestLevels.GetAllAsync().Result
                         .Where(e => e.ContestId.Equals(contest.Id)))
            {
                await _bll.ContestLevels.RemoveAsync(level);
            }

            //Remove contest package
            foreach (var package in _bll.ContestPackages.GetAllAsync().Result
                         .Where(e => e.ContestId.Equals(contest.Id)))
            {
                await _bll.ContestPackages.RemoveAsync(package);
            }

            //Remove contest times
            foreach (var times in _bll.ContestTimes.GetAllAsync().Result.Where(e => e.ContestId.Equals(contest.Id)))
            {
                await _bll.ContestTimes.RemoveAsync(times);
            }

            await _bll.Contests.RemoveAsync(contest);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}