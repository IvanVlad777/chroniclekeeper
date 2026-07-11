using ChronicleKeeper.Core.CQRS.Abilities.Commands;
using ChronicleKeeper.Core.CQRS.Abilities.Queries;
using ChronicleKeeper.Core.DTOs.Ability;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/abilities")]
    [ApiController]
    public class AbilityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AbilityController> _logger;

        public AbilityController(IMediator mediator, ILogger<AbilityController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/abilities?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get abilities", Description = "Returns abilities, optionally filtered by world")]
        [SwaggerResponse(200, "List of abilities", typeof(IEnumerable<AbilityDto>))]
        public async Task<ActionResult<IEnumerable<AbilityDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var abilities = await _mediator.Send(new GetAllAbilitiesQuery { WorldId = worldId });
            return Ok(abilities);
        }

        // GET: /api/abilities/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get ability by ID")]
        [SwaggerResponse(200, "Ability found", typeof(AbilityDto))]
        [SwaggerResponse(404, "Ability not found")]
        public async Task<ActionResult<AbilityDto>> GetById(int id)
        {
            var ability = await _mediator.Send(new GetAbilityByIdQuery { Id = id });
            if (ability == null) return NotFound();
            return Ok(ability);
        }

        // POST: /api/abilities
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new ability")]
        [SwaggerResponse(201, "Ability created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<AbilityDto>> Create([FromBody] AbilityCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating ability: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateAbilityCommand { AbilityCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/abilities/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update ability by ID")]
        [SwaggerResponse(200, "Ability updated", typeof(AbilityDto))]
        [SwaggerResponse(404, "Ability not found")]
        public async Task<ActionResult<AbilityDto>> Update(int id, [FromBody] AbilityUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateAbilityCommand { Id = id, AbilityUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/abilities/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete ability by ID")]
        [SwaggerResponse(204, "Ability deleted")]
        [SwaggerResponse(404, "Ability not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteAbilityCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
