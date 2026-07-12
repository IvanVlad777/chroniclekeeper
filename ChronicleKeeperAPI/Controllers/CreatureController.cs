using ChronicleKeeper.Core.CQRS.Creatures.Commands;
using ChronicleKeeper.Core.CQRS.Creatures.Queries;
using ChronicleKeeper.Core.DTOs.Creature;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/creatures")]
    [ApiController]
    public class CreatureController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreatureController> _logger;

        public CreatureController(IMediator mediator, ILogger<CreatureController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/creatures?worldId=1&subtype=Animal
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get creatures", Description = "Returns creatures (Animal/Plant/Tree/Crop/Fungus), optionally filtered by world and/or subtype")]
        [SwaggerResponse(200, "List of creatures", typeof(IEnumerable<CreatureDto>))]
        public async Task<ActionResult<IEnumerable<CreatureDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] string? subtype = null)
        {
            var creatures = await _mediator.Send(new GetAllCreaturesQuery { WorldId = worldId, Subtype = subtype });
            return Ok(creatures);
        }

        // GET: /api/creatures/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get creature by ID")]
        [SwaggerResponse(200, "Creature found", typeof(CreatureDetailsDto))]
        [SwaggerResponse(404, "Creature not found")]
        public async Task<ActionResult<CreatureDetailsDto>> GetById(int id)
        {
            var creature = await _mediator.Send(new GetCreatureByIdQuery { Id = id });
            if (creature == null) return NotFound();
            return Ok(creature);
        }

        // POST: /api/creatures
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new creature", Description = "Subtype selects the concrete TPH subtype (Animal/Plant/Tree/Crop/Fungus) and cannot be changed afterward")]
        [SwaggerResponse(201, "Creature created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<CreatureDto>> Create([FromBody] CreatureCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating creature: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateCreatureCommand { CreatureCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/creatures/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update creature by ID", Description = "A creature's concrete subtype cannot be changed")]
        [SwaggerResponse(200, "Creature updated", typeof(CreatureDto))]
        [SwaggerResponse(404, "Creature not found")]
        public async Task<ActionResult<CreatureDto>> Update(int id, [FromBody] CreatureUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateCreatureCommand { Id = id, CreatureUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/creatures/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete creature by ID")]
        [SwaggerResponse(204, "Creature deleted")]
        [SwaggerResponse(404, "Creature not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCreatureCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/creatures/{id}/cities/{cityId}
        [HttpPost("{id}/cities/{cityId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a creature to a city it inhabits")]
        [SwaggerResponse(204, "City linked")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<IActionResult> AddCity(int id, int cityId)
        {
            await _mediator.Send(new AddCreatureCityCommand { CreatureId = id, CityId = cityId });
            return NoContent();
        }

        // DELETE: /api/creatures/{id}/cities/{cityId}
        [HttpDelete("{id}/cities/{cityId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a creature from a city")]
        [SwaggerResponse(204, "City unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveCity(int id, int cityId)
        {
            var result = await _mediator.Send(new RemoveCreatureCityCommand { CreatureId = id, CityId = cityId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/creatures/{id}/habitats/{ecosystemId}
        [HttpPost("{id}/habitats/{ecosystemId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a creature to an ecosystem it inhabits")]
        [SwaggerResponse(204, "Ecosystem linked")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<IActionResult> AddHabitat(int id, int ecosystemId)
        {
            await _mediator.Send(new AddCreatureHabitatCommand { CreatureId = id, EcosystemId = ecosystemId });
            return NoContent();
        }

        // DELETE: /api/creatures/{id}/habitats/{ecosystemId}
        [HttpDelete("{id}/habitats/{ecosystemId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a creature from an ecosystem")]
        [SwaggerResponse(204, "Ecosystem unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveHabitat(int id, int ecosystemId)
        {
            var result = await _mediator.Send(new RemoveCreatureHabitatCommand { CreatureId = id, EcosystemId = ecosystemId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
