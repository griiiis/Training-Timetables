using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using App.DTO.v1_0;
using App.DTO.v1_0.DTOs.Contests;
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

        private readonly PublicDTOBllMapper<EditContestDTO, App.BLL.DTO.DTOs.Contests.EditContestDTO>
            _editContestDTOMapper;

        private readonly PublicDTOBllMapper<CreateContestDTO, App.BLL.DTO.DTOs.Contests.CreateContestDTO>
            _createContestDTOMapper;

        private readonly PublicDTOBllMapper<OwnerContestsDTO, App.BLL.DTO.DTOs.Contests.OwnerContestsDTO>
            _ownerContestsDTOMapper;

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
            _editContestDTOMapper =
                new PublicDTOBllMapper<EditContestDTO, App.BLL.DTO.DTOs.Contests.EditContestDTO>(autoMapper);
            _createContestDTOMapper =
                new PublicDTOBllMapper<CreateContestDTO, App.BLL.DTO.DTOs.Contests.CreateContestDTO>(autoMapper);
            _ownerContestsDTOMapper =
                new PublicDTOBllMapper<OwnerContestsDTO, App.BLL.DTO.DTOs.Contests.OwnerContestsDTO>(autoMapper);
        }

        /// <summary>
        /// Returns front page contests
        /// </summary>
        /// <returns>List of contests</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<List<FrontPageContestsDTO>>((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<FrontPageContestsDTO>>> GetFrontPageContests()
        {
            var allContests = new FrontPageContestsDTO
            {
                CurrentContestsDTO = new List<FrontPageContestDTO>(),
                ComingContestsDTO = new List<FrontPageContestDTO>()
            };
            
            var res = (await _bll.Contests.GetAllAsync(default))
                .Select(e => _mapper.Map(e)).ToList();
            
            // Current contests
            foreach (var contest in res.Where(e => e!.From < DateTime.Now && e.Until > DateTime.Now))
            {
                var currentContestDTO = new FrontPageContestDTO
                {
                    Id = contest!.Id,
                    ContestName = contest.ContestName,
                    Description = contest.Description,
                    TotalHours = contest.TotalHours,
                    From = contest.From,
                    Until = contest.Until,
                    LocationName = contest.Location!.LocationName,
                    ContestTypeName = contest.ContestType!.ContestTypeName,
                    NumberOfParticipants = _bll.UserContestPackages.GetContestParticipants(contest.Id).Result.Count(),

                    ContestGameTypes = contest.ContestGameTypes.Select(e => e.GameType!.GameTypeName)
                        .ToList()
                };
                allContests.CurrentContestsDTO.Add(currentContestDTO);
            }
            
            // Coming contests
            foreach (var contest in res.Where(e => e!.From > DateTime.Now))
            {
                var comingContestDTO = new FrontPageContestDTO
                {
                    Id = contest!.Id,
                    ContestName = contest.ContestName,
                    Description = contest.Description,
                    TotalHours = contest.TotalHours,
                    From = contest.From,
                    Until = contest.Until,
                    LocationName = contest.Location!.LocationName,
                    ContestTypeName = contest.ContestType!.ContestTypeName,
                    NumberOfParticipants = _bll.UserContestPackages.GetContestParticipants(contest.Id).Result.Count(),

                    ContestGameTypes = contest.ContestGameTypes.Select(e => e.GameType!.GameTypeName)
                        .ToList()
                };
                allContests.ComingContestsDTO.Add(comingContestDTO);
                
            }
            
            return Ok(allContests);
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
        public async Task<ActionResult<List<OwnerContestsDTO>>> GetOwnerContests()
        {
            var res = (await _bll.Contests.GetOwnerContests(UserId))
                .Select(e => _ownerContestsDTOMapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns all contests visible to current user with all data
        /// </summary>
        /// <returns>List of contests for participant/trainer my contests</returns>
        [HttpGet("user")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<Contest>>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<MyContestsDTO>> GetUserContestsWithAllData()
        {
            var currentContestsList = new List<UserContestsDTO>();
            var comingContestsList = new List<UserContestsDTO>();

            var res = (await _bll.Contests.GetUserContests(UserId))
                .Select(e => _mapper.Map(e)).ToList();

            // Current contests
            foreach (var contest in res.Where(e => e!.From < DateTime.Now && e.Until > DateTime.Now))
            {
                var userPackage = _bll.UserContestPackages.GetUserContestPackage(contest!.Id, UserId).Result;
                var ifTrainer = _bll.ContestUserRoles.IfContestTrainer(UserId, contest.Id);
                var currentContestsDTO = new UserContestsDTO
                {
                    ContestId = contest!.Id,
                    ContestName = contest.ContestName,
                    Description = contest.Description,
                    TotalHours = contest.TotalHours,
                    From = contest.From,
                    Until = contest.Until,
                    LocationName = contest.Location!.LocationName,
                    ContestTypeName = contest.ContestType!.ContestTypeName,
                    AnyGames = _bll.Games.AnyContestGames(contest.Id),
                    TeamId = userPackage!.TeamId,
                    IfTrainer = ifTrainer,
                };
                if (ifTrainer)
                {
                    currentContestsDTO.GameTypesDTOs = _bll.GameTypes.GetAllCurrentContestAsync(contest.Id).Result
                        .Select(e => new GameTypesDTO
                        {
                            GameTypeId = e.Id,
                            GameTypeName = e.GameTypeName
                        }).ToList();

                    currentContestsDTO.RolePreferenceDTOs = _bll.RolePreferences.GetAllAsync(UserId).Result
                        .Select(e => new RolePreferenceDTO
                        {
                            GameTypeId = e.GameTypeId,
                            GameTypeName = e.GameType!.GameTypeName,
                            LevelTitle = e.Level!.Title
                        }).ToList();
                }
                else
                {
                    var teamMates = _bll.UserContestPackages.GetContestTeammates(contest.Id, userPackage!.TeamId).Result
                        .Select(e =>
                            new UserPackagesDTO
                            {
                                PackageId = e.Id,
                                FirstName = e.AppUser!.FirstName,
                                LastName = e.AppUser.LastName
                            }).ToList();
                    
                    currentContestsDTO.LevelTitle = userPackage.Level!.Title;
                    currentContestsDTO.GameTypeName = userPackage.PackageGameTypeTime!.GameType!.GameTypeName;
                    currentContestsDTO.PackageName = userPackage.PackageGameTypeTime.PackageGtName;
                    currentContestsDTO.PackagesDTOs = teamMates;
                }

                currentContestsList.Add(currentContestsDTO);
            }

            foreach (var contest in res.Where(e => e!.From > DateTime.Now))
            {
                var userPackage = _bll.UserContestPackages.GetUserContestPackage(contest!.Id, UserId).Result;
                var ifTrainer = _bll.ContestUserRoles.IfContestTrainer(UserId, contest.Id);
                
                var comingContestsDTO = new UserContestsDTO
                {
                    ContestId = contest!.Id,
                    ContestName = contest.ContestName,
                    Description = contest.Description,
                    TotalHours = contest.TotalHours,
                    From = contest.From,
                    Until = contest.Until,
                    LocationName = contest.Location!.LocationName,
                    ContestTypeName = contest.ContestType!.ContestTypeName,
                    AnyGames = _bll.Games.AnyContestGames(contest.Id),
                    TeamId = userPackage!.TeamId,
                    IfTrainer = ifTrainer,
                };
                if (ifTrainer)
                {
                    comingContestsDTO.GameTypesDTOs = _bll.GameTypes.GetAllCurrentContestAsync(contest.Id).Result
                        .Select(e => new GameTypesDTO
                        {
                            GameTypeId = e.Id,
                            GameTypeName = e.GameTypeName
                        }).ToList();

                    comingContestsDTO.RolePreferenceDTOs = _bll.RolePreferences.GetAllAsync(UserId).Result
                        .Select(e => new RolePreferenceDTO
                        {
                            GameTypeId = e.GameTypeId,
                            GameTypeName = e.GameType!.GameTypeName,
                            LevelTitle = e.Level!.Title
                        }).ToList();
                }
                else
                {
                    var teamMates = _bll.UserContestPackages.GetContestTeammates(contest.Id, userPackage!.TeamId).Result
                        .Select(e =>
                            new UserPackagesDTO
                            {
                                PackageId = e.Id,
                                FirstName = e.AppUser!.FirstName,
                                LastName = e.AppUser.LastName
                            }).ToList();
                    comingContestsDTO.LevelTitle = userPackage.Level!.Title;
                    comingContestsDTO.GameTypeName = userPackage.PackageGameTypeTime!.GameType!.GameTypeName;
                    comingContestsDTO.PackageName = userPackage.PackageGameTypeTime.PackageGtName;
                    comingContestsDTO.PackagesDTOs = teamMates;
                }
                comingContestsList.Add(comingContestsDTO);
            }

            var myContestsDTO = new MyContestsDTO
            {
                CurrentContestsDTO = currentContestsList,
                ComingContestsDTO = comingContestsList
            };

            return Ok(myContestsDTO);
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
        public async Task<ActionResult<InformationContestDTO>> GetContest(Guid id)
        {
            var contest = _mapper.Map(await _bll.Contests.FirstOrDefaultAsync(id));

            if (contest == null)
            {
                return NotFound();
            }

            var contestDTO = new InformationContestDTO
            {
                Id = contest.Id,
                ContestName = contest.ContestName,
                Description = contest.Description,
                TotalHours = contest.TotalHours,
                From = contest.From,
                Until = contest.Until,
                LocationName = contest.Location!.LocationName,
                ContestTypeName = contest.ContestType!.ContestTypeName
            };

            return Ok(contestDTO);
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
        public async Task<ActionResult<DeleteContestDTO>> GetOwnerContest(Guid id)
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

            var deleteContestDTO = new DeleteContestDTO
            {
                Id = contest.Id,
                ContestName = contest.ContestName,
                Description = contest.Description,
                TotalHours = contest.TotalHours,
                From = contest.From,
                Until = contest.Until,
                LocationName = contest.Location!.LocationName,
                ContestTypeName = contest.ContestType!.ContestTypeName
            };

            return Ok(deleteContestDTO);
        }

        [HttpGet("owner/edit/{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<Contest>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<EditContestDTO>> EditContest(Guid id)
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

            var vm = _editContestDTOMapper.Map(await _bll.Contests.GetContestEditModel(UserId, id));

            return Ok(vm);
        }

        /// <summary>
        /// Edit user's contest
        /// </summary>
        /// <param name="id">Contest Id</param>
        /// <param name="editContestDto">Contest DTO</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<IActionResult> PutContest(Guid id, EditContestDTO editContestDto)
        {
            if (id != editContestDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _bll.Contests.PutContest(UserId, id, _editContestDTOMapper.Map(editContestDto));
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
        public async Task<ActionResult<Contest>> PostContest(CreateContestDTO createContestDTO)
        {
            if (createContestDTO.SelectedLevelIds!.Count < 1 || createContestDTO.SelectedTimesIds!.Count < 1 ||
                createContestDTO.SelectedPackagesIds!.Count < 1)
            {
                return NotFound("Required forms not filled!");
            }

            var newContest = await _bll.Contests.PostContest(UserId, _createContestDTOMapper.Map(createContestDTO));

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
        [HttpDelete("{id:guid}")]
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

            //Remove contest roles
            foreach (var role in _bll.ContestRoles.GetAllAsync().Result.Where(e => e.ContestId.Equals(contest.Id)))
            {
                await _bll.ContestRoles.RemoveAsync(role);
            }

            await _bll.Contests.RemoveAsync(contest);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}