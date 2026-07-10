using ChronicleKeeper.Core.CQRS.PoliticalParties.Commands;
using ChronicleKeeper.Core.CQRS.PoliticalParties.Queries;
using ChronicleKeeper.Core.DTOs.PoliticalParty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/politicalparties")]
    [ApiController]
    public class PoliticalPartyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PoliticalPartyController> _logger;

        public PoliticalPartyController(IMediator mediator, ILogger<PoliticalPartyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/politicalparties?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get political parties", Description = "Returns political parties, optionally filtered by world")]
        [SwaggerResponse(200, "List of political parties", typeof(IEnumerable<PoliticalPartyDto>))]
        public async Task<ActionResult<IEnumerable<PoliticalPartyDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var parties = await _mediator.Send(new GetAllPoliticalPartiesQuery { WorldId = worldId });
            return Ok(parties);
        }

        // GET: /api/politicalparties/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get political party by ID", Description = "Returns political party with its ideology and government system")]
        [SwaggerResponse(200, "Political party found", typeof(PoliticalPartyDetailsDto))]
        [SwaggerResponse(404, "Political party not found")]
        public async Task<ActionResult<PoliticalPartyDetailsDto>> GetById(int id)
        {
            var party = await _mediator.Send(new GetPoliticalPartyByIdQuery { Id = id });
            if (party == null) return NotFound();
            return Ok(party);
        }

        // POST: /api/politicalparties
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new political party")]
        [SwaggerResponse(201, "Political party created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<PoliticalPartyDto>> Create([FromBody] PoliticalPartyCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating political party: {Name}", dto.Name);
            var result = await _mediator.Send(new CreatePoliticalPartyCommand { PoliticalPartyCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/politicalparties/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update political party by ID")]
        [SwaggerResponse(200, "Political party updated", typeof(PoliticalPartyDto))]
        [SwaggerResponse(404, "Political party not found")]
        public async Task<ActionResult<PoliticalPartyDto>> Update(int id, [FromBody] PoliticalPartyUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdatePoliticalPartyCommand { Id = id, PoliticalPartyUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/politicalparties/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete political party by ID")]
        [SwaggerResponse(204, "Political party deleted")]
        [SwaggerResponse(404, "Political party not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePoliticalPartyCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
