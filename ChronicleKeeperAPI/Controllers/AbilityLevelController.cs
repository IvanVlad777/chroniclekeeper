using ChronicleKeeper.Core.CQRS.AbilityLevels.Commands;
using ChronicleKeeper.Core.CQRS.AbilityLevels.Queries;
using ChronicleKeeper.Core.DTOs.AbilityLevel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/ability-levels")]
    [ApiController]
    public class AbilityLevelController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AbilityLevelController> _logger;

        public AbilityLevelController(IMediator mediator, ILogger<AbilityLevelController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/ability-levels?worldId=1&abilityId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get ability levels", Description = "Returns ability levels, optionally filtered by world and/or ability")]
        [SwaggerResponse(200, "List of ability levels", typeof(IEnumerable<AbilityLevelDto>))]
        public async Task<ActionResult<IEnumerable<AbilityLevelDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? abilityId = null)
        {
            var levels = await _mediator.Send(new GetAllAbilityLevelsQuery { WorldId = worldId, AbilityId = abilityId });
            return Ok(levels);
        }

        // GET: /api/ability-levels/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get ability level by ID")]
        [SwaggerResponse(200, "Ability level found", typeof(AbilityLevelDto))]
        [SwaggerResponse(404, "Ability level not found")]
        public async Task<ActionResult<AbilityLevelDto>> GetById(int id)
        {
            var level = await _mediator.Send(new GetAbilityLevelByIdQuery { Id = id });
            if (level == null) return NotFound();
            return Ok(level);
        }

        // POST: /api/ability-levels
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new ability level", Description = "Ability level's world is derived from its ability")]
        [SwaggerResponse(201, "Ability level created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<AbilityLevelDto>> Create([FromBody] AbilityLevelCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating ability level: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateAbilityLevelCommand { AbilityLevelCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/ability-levels/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update ability level by ID", Description = "An ability level's ability cannot be changed")]
        [SwaggerResponse(200, "Ability level updated", typeof(AbilityLevelDto))]
        [SwaggerResponse(404, "Ability level not found")]
        public async Task<ActionResult<AbilityLevelDto>> Update(int id, [FromBody] AbilityLevelUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateAbilityLevelCommand { Id = id, AbilityLevelUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/ability-levels/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete ability level by ID")]
        [SwaggerResponse(204, "Ability level deleted")]
        [SwaggerResponse(404, "Ability level not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteAbilityLevelCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
