using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using App.DTO.v1_0;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly PublicDTOBllMapper<Level, App.BLL.DTO.Level> _levelMapper;
        private readonly PublicDTOBllMapper<PackageGameTypeTime, App.BLL.DTO.PackageGameTypeTime> _packagesMapper;
        private readonly PublicDTOBllMapper<Time, App.BLL.DTO.Time> _timesMapper;
        private readonly PublicDTOBllMapper<Location, App.BLL.DTO.Location> _locationMapper;
        private readonly PublicDTOBllMapper<ContestType, App.BLL.DTO.ContestType> _contestTypeMapper;

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
            _contestTypeMapper = new PublicDTOBllMapper<ContestType, App.BLL.DTO.ContestType>(autoMapper);
            _levelMapper = new PublicDTOBllMapper<Level, App.BLL.DTO.Level>(autoMapper);
            _locationMapper = new PublicDTOBllMapper<Location, App.BLL.DTO.Location>(autoMapper);
            _packagesMapper = new PublicDTOBllMapper<PackageGameTypeTime, App.BLL.DTO.PackageGameTypeTime>(autoMapper);
            _timesMapper = new PublicDTOBllMapper<Time, App.BLL.DTO.Time>(autoMapper);
            _mapper = new PublicDTOBllMapper<Contest, App.BLL.DTO.Contest>(autoMapper);
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

            var previousLevels = (await _bll.Contests.FirstOrDefaultAsync(id, UserId))!.ContestLevels
                .Select(e => e.Level).Select(de => _levelMapper.Map(de)).ToList();

            var previousPackages = (await _bll.Contests.FirstOrDefaultAsync(id, UserId))!.ContestPackages
                .Select(e => e.PackageGameTypeTime).Select(de => _packagesMapper.Map(de)).ToList();

            var previousTimes = (await _bll.Contests.FirstOrDefaultAsync(id, UserId))!.ContestTimes
                .Select(e => e.Time).Select(de => _timesMapper.Map(de)).ToList();

            var vm = new ContestEditModel()
            {
                Contest = contest,
                ContestTypeList = (await _bll.ContestTypes.GetAllAsync(UserId))
                    .Select(de => _contestTypeMapper.Map(de))
                    .ToList(),
                LocationList = (await _bll.Locations.GetAllAsync(UserId)).Select(de => _locationMapper.Map(de))
                    .ToList(),
                LevelList = (await _bll.Levels.GetAllAsync(UserId)).Select(de => _levelMapper.Map(de)).ToList(),
                TimesList =
                    (await _bll.Times.GetAllAsync(UserId)).Select(de => _timesMapper.Map(de)).ToList(),
                PackagesList =
                    (await _bll.PackageGameTypeTimes.GetAllAsync(UserId))
                    .Select(de => _packagesMapper.Map(de)).ToList(),
                PreviousLevels = previousLevels,
                PreviousPackages = previousPackages!,
                PreviousTimes = previousTimes!,
            };

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
                //Remove previous packages
                var previousPackages = _bll.ContestPackages.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                foreach (var package in previousPackages)
                {
                    await _bll.ContestPackages.RemoveAsync(package);
                }

                //Remove previous gameTypes
                var previousGameTypes =
                    _bll.ContestGameTypes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                foreach (var gameType in previousGameTypes)
                {
                    await _bll.ContestGameTypes.RemoveAsync(gameType);
                }

                //Remove times
                var previousTimes = _bll.ContestTimes.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                foreach (var time in previousTimes)
                {
                    await _bll.ContestTimes.RemoveAsync(time);
                }

                //Remove levels
                var previousLevels = _bll.ContestLevels.GetAllAsync().Result.Where(e => e.ContestId.Equals(id));
                foreach (var level in previousLevels)
                {
                    await _bll.ContestLevels.RemoveAsync(level);
                }

                var gameTypes = new HashSet<Guid>();
                var allPackages = (await _bll.PackageGameTypeTimes.GetAllAsync(default)).ToList();

                foreach (var packageId in contest.SelectedPackagesIds!)
                {
                    var gameTypeId = allPackages
                        .FirstOrDefault(e => e.Id.Equals(packageId) && !gameTypes.Contains(e.GameTypeId))
                        ?.GameTypeId;

                    if (gameTypeId != null)
                    {
                        gameTypes.Add(gameTypeId.Value);
                    }
                }

                foreach (var gameTypeId in gameTypes)
                {
                    var contestGameType = new App.BLL.DTO.ContestGameType()
                    {
                        ContestId = contest.Contest.Id,
                        GameTypeId = gameTypeId
                    };
                    _bll.ContestGameTypes.Add(contestGameType);
                }

                foreach (var timeId in contest.SelectedTimesIds!)
                {
                    var times = new App.BLL.DTO.ContestTime()
                    {
                        ContestId = contest.Contest.Id,
                        TimeId = timeId
                    };
                    _bll.ContestTimes.Add(times);
                }

                foreach (var packageId in contest.SelectedPackagesIds!)
                {
                    var package = new App.BLL.DTO.ContestPackage()
                    {
                        ContestId = contest.Contest.Id,
                        PackageGameTypeTimeId = packageId
                    };
                    _bll.ContestPackages.Add(package);
                }

                foreach (var levelId in contest.SelectedLevelIds!)
                {
                    var contestLevel = new App.BLL.DTO.ContestLevel()
                    {
                        ContestId = contest.Contest.Id,
                        LevelId = levelId
                    };
                    _bll.ContestLevels.Add(contestLevel);
                }


                _bll.Contests.UpdateContestWithUser(UserId, _mapper.Map(contest.Contest)!);
                await _bll.SaveChangesAsync();
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
        public async Task<ActionResult<Contest>> PostContest(ContestEditModel contest)
        {
            if (contest.SelectedLevelIds!.Count < 1 || contest.SelectedTimesIds!.Count < 1 ||
                contest.SelectedPackagesIds!.Count < 1)
            {
                return NotFound("Required forms not filled!");
            }

            contest.Contest.Id = Guid.NewGuid();
            contest.Contest.From = contest.Contest.From.ToUniversalTime();
            contest.Contest.Until = contest.Contest.Until.ToUniversalTime();

            foreach (var levelId in contest.SelectedLevelIds!)
            {
                var contestLevel = new App.BLL.DTO.ContestLevel
                {
                    ContestId = contest.Contest.Id,
                    LevelId = levelId
                };
                _bll.ContestLevels.Add(contestLevel);
            }

            var gameTypes = new HashSet<Guid>();
            var allPackages = (await _bll.PackageGameTypeTimes.GetAllAsync(default)).ToList();

            foreach (var id in contest.SelectedPackagesIds)
            {
                var gameTypeId = allPackages
                    .FirstOrDefault(e => e.Id.Equals(id) && !gameTypes.Contains(e.GameTypeId))
                    ?.GameTypeId;

                if (gameTypeId != null)
                {
                    gameTypes.Add(gameTypeId.Value);
                }
            }

            foreach (var gameTypeId in gameTypes)
            {
                var contestGameType = new App.BLL.DTO.ContestGameType()
                {
                    ContestId = contest.Contest.Id,
                    GameTypeId = gameTypeId
                };
                _bll.ContestGameTypes.Add(contestGameType);
            }

            foreach (var timeId in contest.SelectedTimesIds!)
            {
                var timeOfDay = new App.BLL.DTO.ContestTime()
                {
                    ContestId = contest.Contest.Id,
                    TimeId = timeId
                };
                _bll.ContestTimes.Add(timeOfDay);
            }

            foreach (var packageId in contest.SelectedPackagesIds!)
            {
                var package = new App.BLL.DTO.ContestPackage()
                {
                    ContestId = contest.Contest.Id,
                    PackageGameTypeTimeId = packageId
                };
                _bll.ContestPackages.Add(package);
            }

            var newContest = _bll.Contests.AddContestWithUser(UserId, _mapper.Map(contest.Contest)!);
            await _bll.SaveChangesAsync();

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