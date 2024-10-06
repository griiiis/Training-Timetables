using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using App.DTO.v1_0;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Invitation Api Controller
    /// </summary>
    [ApiVersion(("1.0"))]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<Invitation, App.BLL.DTO.Invitation> _mapper;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public InvitationsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<Invitation, App.BLL.DTO.Invitation>(autoMapper);
        }

        // GET: api/Invitations
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType<List<Invitation>>((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Invitation>>> GetInvitations()
        {
            return (await _bll.Invitations.GetAllAsync()).Select(e => _mapper.Map(e)).ToList();
        }

        // GET: api/Invitations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invitation>> GetInvitation(Guid id)
        {
            var invitation = await _bll.Invitations.FirstOrDefaultAsync(id);

            if (invitation == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(invitation));
        }

        // PUT: api/Invitations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvitation(Guid id, Invitation invitation)
        {
            if (id != invitation.Id)
            {
                return BadRequest();
            }

            _bll.Invitations.Update(_mapper.Map(invitation));

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Invitations.ExistsAsync(id))
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

        // POST: api/Invitations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invitation>> PostInvitation(Invitation invitation)
        {
            _bll.Invitations.Add(_mapper.Map(invitation));
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetInvitation", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = invitation.Id
            }, invitation);
        }

        // DELETE: api/Invitations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvitation(Guid id)
        {
            var invitation = await _bll.Invitations.FirstOrDefaultAsync(id);
            if (invitation == null)
            {
                return NotFound();
            }

            await _bll.Invitations.RemoveAsync(invitation);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
