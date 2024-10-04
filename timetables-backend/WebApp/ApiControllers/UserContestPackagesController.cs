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
    /// UserContestPackage Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class UserContestPackagesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.UserContestPackage, UserContestPackage> _mapper;
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        /// <summary>
        /// Contest constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="userManager">User Manager</param>
        /// <param name="autoMapper">AutoMapper</param>
        /// <param name="roleManager">Role Manager</param>
        public UserContestPackagesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper, RoleManager<AppRole> roleManager)
        {
            _bll = bll;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.UserContestPackage, UserContestPackage>(autoMapper);
        }
        
        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.UserContestPackage>>((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<App.DTO.v1_0.UserContestPackage>>> GetAllUsers()
        {
            var res = (await _bll.UserContestPackages.GetAllAsync(default))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("user")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.UserContestPackage>>((int)HttpStatusCode.OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DTO.v1_0.UserContestPackage>>> GetCurrentUserPackages()
        {
            var res = (await _bll.UserContestPackages.GetCurrentUserPackages(UserId))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        
        //GET CURRENT USER ALL USERPACKAGES
        
        /// <summary>
        /// Returns all users that are taking part in the contest
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("contestusers/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.UserContestPackage>>((int)HttpStatusCode.OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DTO.v1_0.UserContestPackage>>> GetUsers(Guid contestId)
        {
            var res = (await _bll.UserContestPackages.GetContestUsers(contestId))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        
        /// <summary>
        /// Return boolean if any teams have joined with contest
        /// </summary>
        /// <returns>Boolean</returns>
        [HttpGet("teams/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<bool>((int)HttpStatusCode.OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<bool> AnyTeams(Guid contestId)
        {
            return Ok(_bll.UserContestPackages.AnyTeams(contestId));
        }
        
        /// <summary>
        /// Returns all users that are participating in that contest without Trainers
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("users/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.UserContestPackage>((int)HttpStatusCode.OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DTO.v1_0.UserContestPackage>>> GetContestParticipants(Guid contestId)
        {
            var res = (await _bll.UserContestPackages.GetContestParticipants(contestId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns all users that are part of that contest and team
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("users/{contestId:guid}/{teamId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.UserContestPackage>((int)HttpStatusCode.OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DTO.v1_0.UserContestPackage>>> GetContestTeammates(Guid contestId, Guid teamId)
        {
            var res = (await _bll.UserContestPackages.GetContestTeammates(contestId, teamId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns all Trainers that are participating in that contest
        /// </summary>
        /// <returns>List of trainers</returns>
        [HttpGet("teachers/{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.UserContestPackage>((int)HttpStatusCode.OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DTO.v1_0.UserContestPackage>>> GetContestTeachers(Guid contestId)
        {
            var res = (await _bll.UserContestPackages.GetContestTeachers(contestId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Returns all userContestPackages
        /// </summary>
        /// <returns>List of userContestPackages</returns>
        [HttpGet("owner")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.UserContestPackage>((int)HttpStatusCode.OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Contest Admin")]
        public async Task<ActionResult<List<App.DTO.v1_0.UserContestPackage>>> GetUserContestPackages()
        {
            var res = (await _bll.UserContestPackages.GetAllAsync(UserId)).Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }

        /// <summary>
        /// Returns userContestPackage that matches given id
        /// </summary>
        /// <param name="id">User Contest Package Id</param>
        /// <returns>User Contest Package that matches given id</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.UserContestPackage>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DTO.v1_0.UserContestPackage>> GetUserContestPackage(Guid id)
        {
            var userContestPackage = _mapper.Map(await _bll.UserContestPackages.GetUserContestPackage(id, UserId));
            if (userContestPackage == null)
            {
                return NotFound();
            }
            return Ok(userContestPackage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">User Contest Package Id</param>
        /// <param name="userContestPackage">User Contest Package</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUserContestPackage(Guid id, App.DTO.v1_0.UserContestPackage userContestPackage)
        {
            if (id != userContestPackage.Id)
            {
                return BadRequest();
            }
            _bll.UserContestPackages.Update(_mapper.Map(userContestPackage)!);
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.UserContestPackages.ExistsAsync(id))
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
        /// Adds new UserContestPackage 
        /// </summary>
        /// <param name="userContestPackage">User contest package</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.UserContestPackage>((int)HttpStatusCode.Created)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DTO.v1_0.UserContestPackage>> PostUserContestPackage(App.DTO.v1_0.UserContestPackage userContestPackage)
        {
            var bllPackage = _mapper.Map(userContestPackage)!;
            
            //Osavõtja roll
                var user = await _userManager.GetUserAsync(User);
                await _userManager.RemoveFromRolesAsync(user!,
                    (await _userManager.GetRolesAsync(user!)).ToList());
                
                //
                await _userManager.AddToRoleAsync(user!, "Participant");
                
                var packageGameType = await _bll.PackageGameTypeTimes.FirstOrDefaultAsync(bllPackage.PackageGameTypeTimeId);
                var contest = await _bll.Contests.FirstOrDefaultAsync(bllPackage.ContestId);
                var level = await _bll.Levels.FirstOrDefaultAsync(bllPackage.LevelId);
                
                var team = new Team()  // Create team
                {
                    Id = Guid.NewGuid(),
                    TeamName = _userManager.GetUserName(User) + " tiim",
                    LevelId = level!.Id,
                    GameTypeId = packageGameType!.GameTypeId,
                };
                _bll.Teams.Add(team);
                
                bllPackage.Id = Guid.NewGuid();
                bllPackage.HoursAvailable = contest!.TotalHours * packageGameType.Times;
                bllPackage.AppUserId = Guid.Parse(_userManager.GetUserId(User));
                bllPackage.TeamId = team.Id;
                var newPackage = _bll.UserContestPackages.Add(bllPackage);
                await _bll.SaveChangesAsync();
                /*
                //Create all times for team
                var timeOfDays = _bll.TimeOfDays.GetAllAsync().Result.ToList();
                var contestStartDate = contest.From.Date;
                var contestEndDate = contest.Until.Date;
                var contestLength = (contestEndDate - contestStartDate).TotalDays;

                for (int i = 0; i <= contestLength; i++)
                {
                    foreach (var timeOfDay in timeOfDays)
                    {
                        _bll.TimeTeams.Add(new TimeTeam
                        {
                            TimeOfDayId = timeOfDay.Id,
                            Day = DateOnly.FromDateTime (contestStartDate.AddDays(i)),
                            TeamId = team.Id,
                        });
                    }
                }
                */
                
                await _bll.SaveChangesAsync();
            return CreatedAtAction("GetUserContestPackage", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = newPackage.Id
            }, _mapper.Map(newPackage));
        }
        
        /// <summary>
        /// Adds person to Team 
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddToTeam/{teamId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.UserContestPackage>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<App.DTO.v1_0.UserContestPackage>> PostUserToTeam(App.DTO.v1_0.UserContestPackage userContestPackage, Guid teamId)
        {
            var package = await _bll.UserContestPackages.FirstOrDefaultAsync(userContestPackage.Id);
            var appUserId = package!.AppUserId;

            var newPackage = new UserContestPackage
            {
                PackageGameTypeTimeId = package.PackageGameTypeTimeId,
                HoursAvailable = 0,
                ContestId = package.ContestId,
                TeamId = teamId,
                LevelId = package.LevelId,
            };

            _bll.UserContestPackages.AddPackageWithUser(appUserId, newPackage);

            //Get Previous Team
            var previousTeamId =
                _bll.UserContestPackages.GetUserContestPackage(userContestPackage.ContestId, appUserId).Result!.TeamId;
                
            //Delete previous TimeTeams
            await _bll.TimeTeams.RemoveTeamTimeTeamsAsync(previousTeamId);
                
            //Delete previous package
            await _bll.UserContestPackages.RemoveAsync(package);
                
            //Delete previous Team
            await _bll.Teams.RemoveAsync(previousTeamId);
                
            await _bll.SaveChangesAsync();
            return CreatedAtAction("GetuserContestPackage", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = userContestPackage.Id
            }, userContestPackage);
        }

        /// <summary>
        /// Deletes UserContestPackage that matches Id
        /// </summary>
        /// <param name="id">UserContestPackage Id</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUserContestPackage(Guid id)
        {
            var userContestPackage = await _bll.UserContestPackages.FirstOrDefaultAsync(id);
            if (userContestPackage == null)
            {
                return NotFound();
            }
            await _bll.UserContestPackages.RemoveAsync(userContestPackage);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}