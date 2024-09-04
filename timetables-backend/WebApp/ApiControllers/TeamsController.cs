using System.Net;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Team Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeamsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Team, Team> _mapper;

        /// <summary>
        /// Team constructor
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="autoMapper">AutoMapper</param>
        public TeamsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Team, Team>(autoMapper);
        }

        /// <summary>
        /// Returns all contest's teams
        /// </summary>
        /// <param name="contestId">Contest Id</param>
        /// <returns>List of teams</returns>
        [HttpGet("{contestId:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<App.DTO.v1_0.Team>((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<App.DTO.v1_0.Team>>> GetContestTeams(Guid contestId)
        {
            var res = (await _bll.Teams.GetAllCurrentContestAsync(contestId))
                .Select(e => _mapper.Map(e)).ToList();
            return Ok(res);
        }
    }
}
