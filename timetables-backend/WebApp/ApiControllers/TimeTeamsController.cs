using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion(("1.0"))]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimeTeamsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.TimeTeam, TimeTeam> _mapper;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public TimeTeamsController(AppDbContext context, IAppBLL bll, UserManager<AppUser> userManager,
            IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.TimeTeam, TimeTeam>(autoMapper);
        }

        // GET: api/TimeTeam
        [HttpGet("{teamId:guid}")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.TimeTeam>>> GetTimeTeams(Guid teamId)
        {
            return Ok(await _bll.TimeTeams.GetContestTeamTimes(teamId));
        }
    }
}