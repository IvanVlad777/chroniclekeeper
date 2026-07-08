using ChronicleKeeper.Core.CQRS.Factions.Commands;
using ChronicleKeeper.Core.CQRS.Factions.Queries;
using ChronicleKeeper.Core.DTOs.Faction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/factions")]
    [ApiController]
    public class FactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FactionController> _logger;

        public FactionController(IMediator mediator, ILogger<FactionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/factions?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get factions", Description = "Returns factions, optionally filtered by world")]
        [SwaggerResponse(200, "List of factions", typeof(IEnumerable<FactionDto>))]
        public async Task<ActionResult<IEnumerable<FactionDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var factions = await _mediator.Send(new GetAllFactionsQuery { WorldId = worldId });
            return Ok(factions);
        }

        // GET: /api/factions/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get faction by ID", Description = "Returns detailed faction info (leader, HQ, members, tags)")]
        [SwaggerResponse(200, "Faction found", typeof(FactionDetailsDto))]
        [SwaggerResponse(404, "Faction not found")]
        public async Task<ActionResult<FactionDetailsDto>> GetById(int id)
        {
            var faction = await _mediator.Send(new GetFactionByIdQuery { Id = id });
            if (faction == null) return NotFound();
            return Ok(faction);
        }

        // POST: /api/factions
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new faction")]
        [SwaggerResponse(201, "Faction created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<FactionDto>> Create([FromBody] FactionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating faction: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateFactionCommand { FactionCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/factions/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update faction by ID")]
        [SwaggerResponse(200, "Faction updated", typeof(FactionDto))]
        [SwaggerResponse(404, "Faction not found")]
        public async Task<ActionResult<FactionDto>> Update(int id, [FromBody] FactionUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateFactionCommand { Id = id, FactionUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/factions/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete faction by ID", Description = "Memberships and tag links are removed automatically")]
        [SwaggerResponse(204, "Faction deleted")]
        [SwaggerResponse(404, "Faction not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteFactionCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/factions/{id}/members
        [HttpPost("{id}/members")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Add member to faction", Description = "Character must belong to the faction's world")]
        [SwaggerResponse(201, "Member added", typeof(FactionMemberDto))]
        [SwaggerResponse(400, "Invalid input / already a member")]
        [SwaggerResponse(404, "Faction not found")]
        public async Task<ActionResult<FactionMemberDto>> AddMember(int id, [FromBody] FactionMemberAddDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new AddFactionMemberCommand { FactionId = id, MemberDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, result);
        }

        // DELETE: /api/factions/{id}/members/{characterId}
        [HttpDelete("{id}/members/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove member from faction")]
        [SwaggerResponse(204, "Member removed")]
        [SwaggerResponse(404, "Membership not found")]
        public async Task<IActionResult> RemoveMember(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveFactionMemberCommand { FactionId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
