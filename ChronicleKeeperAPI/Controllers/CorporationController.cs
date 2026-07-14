using ChronicleKeeper.Core.CQRS.CorporateLeaderships.Commands;
using ChronicleKeeper.Core.CQRS.CorporateLeaderships.Queries;
using ChronicleKeeper.Core.CQRS.Corporations.Commands;
using ChronicleKeeper.Core.CQRS.Corporations.Queries;
using ChronicleKeeper.Core.DTOs.CorporateLeadership;
using ChronicleKeeper.Core.DTOs.Corporation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/corporations")]
    [ApiController]
    public class CorporationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CorporationController> _logger;

        public CorporationController(IMediator mediator, ILogger<CorporationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get corporations", Description = "Returns corporations, optionally filtered by world")]
        [SwaggerResponse(200, "List of corporations", typeof(IEnumerable<CorporationDto>))]
        public async Task<ActionResult<IEnumerable<CorporationDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllCorporationsQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get corporation by ID", Description = "Returns corporation with its systems, parent/subsidiaries, leadership and cross-links")]
        [SwaggerResponse(200, "Corporation found", typeof(CorporationDetailsDto))]
        [SwaggerResponse(404, "Corporation not found")]
        public async Task<ActionResult<CorporationDetailsDto>> GetById(int id)
        {
            var corporation = await _mediator.Send(new GetCorporationByIdQuery { Id = id });
            if (corporation == null) return NotFound();
            return Ok(corporation);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new corporation")]
        [SwaggerResponse(201, "Corporation created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<CorporationDto>> Create([FromBody] CorporationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating corporation: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateCorporationCommand { CorporationCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update corporation by ID")]
        [SwaggerResponse(200, "Corporation updated", typeof(CorporationDto))]
        [SwaggerResponse(404, "Corporation not found")]
        public async Task<ActionResult<CorporationDto>> Update(int id, [FromBody] CorporationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateCorporationCommand { Id = id, CorporationUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete corporation by ID", Description = "Blocked while the corporation has subsidiaries")]
        [SwaggerResponse(204, "Corporation deleted")]
        [SwaggerResponse(400, "Corporation has subsidiaries")]
        [SwaggerResponse(404, "Corporation not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCorporationCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/corporations/{id}/factions/{factionId}
        [HttpPost("{id}/factions/{factionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a faction to the corporation")]
        [SwaggerResponse(204, "Faction linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddFaction(int id, int factionId)
        {
            await _mediator.Send(new AddCorporationFactionCommand { CorporationId = id, FactionId = factionId });
            return NoContent();
        }

        // DELETE: /api/corporations/{id}/factions/{factionId}
        [HttpDelete("{id}/factions/{factionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a faction from the corporation")]
        [SwaggerResponse(204, "Faction unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveFaction(int id, int factionId)
        {
            var result = await _mediator.Send(new RemoveCorporationFactionCommand { CorporationId = id, FactionId = factionId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/corporations/{id}/professions/{professionId}
        [HttpPost("{id}/professions/{professionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a profession to the corporation")]
        [SwaggerResponse(204, "Profession linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddProfession(int id, int professionId)
        {
            await _mediator.Send(new AddCorporationProfessionCommand { CorporationId = id, ProfessionId = professionId });
            return NoContent();
        }

        // DELETE: /api/corporations/{id}/professions/{professionId}
        [HttpDelete("{id}/professions/{professionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a profession from the corporation")]
        [SwaggerResponse(204, "Profession unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveProfession(int id, int professionId)
        {
            var result = await _mediator.Send(new RemoveCorporationProfessionCommand { CorporationId = id, ProfessionId = professionId });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/corporate-leaderships")]
    [ApiController]
    public class CorporateLeadershipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CorporateLeadershipController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get corporate leadership entries", Description = "Returns leadership entries, optionally filtered by world and/or corporation")]
        [SwaggerResponse(200, "List of leadership entries", typeof(IEnumerable<CorporateLeadershipDto>))]
        public async Task<ActionResult<IEnumerable<CorporateLeadershipDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? corporationId = null)
        {
            return Ok(await _mediator.Send(new GetAllCorporateLeadershipsQuery { WorldId = worldId, CorporationId = corporationId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get corporate leadership entry by ID")]
        [SwaggerResponse(200, "Leadership entry found", typeof(CorporateLeadershipDto))]
        [SwaggerResponse(404, "Leadership entry not found")]
        public async Task<ActionResult<CorporateLeadershipDto>> GetById(int id)
        {
            var leadership = await _mediator.Send(new GetCorporateLeadershipByIdQuery { Id = id });
            if (leadership == null) return NotFound();
            return Ok(leadership);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new corporate leadership entry", Description = "The entry's world is derived from its corporation")]
        [SwaggerResponse(201, "Leadership entry created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<CorporateLeadershipDto>> Create([FromBody] CorporateLeadershipCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateCorporateLeadershipCommand { CorporateLeadershipCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update corporate leadership entry by ID")]
        [SwaggerResponse(200, "Leadership entry updated", typeof(CorporateLeadershipDto))]
        [SwaggerResponse(404, "Leadership entry not found")]
        public async Task<ActionResult<CorporateLeadershipDto>> Update(int id, [FromBody] CorporateLeadershipUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateCorporateLeadershipCommand { Id = id, CorporateLeadershipUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete corporate leadership entry by ID")]
        [SwaggerResponse(204, "Leadership entry deleted")]
        [SwaggerResponse(404, "Leadership entry not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCorporateLeadershipCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
