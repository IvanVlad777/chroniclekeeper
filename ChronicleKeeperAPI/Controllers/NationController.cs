using ChronicleKeeper.Core.CQRS.Nations.Commands;
using ChronicleKeeper.Core.CQRS.Nations.Queries;
using ChronicleKeeper.Core.DTOs.Nation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/nations")]
    [ApiController]
    public class NationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NationController> _logger;

        public NationController(IMediator mediator, ILogger<NationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/nations?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get nations", Description = "Returns nations, optionally filtered by world")]
        [SwaggerResponse(200, "List of nations", typeof(IEnumerable<NationDto>))]
        public async Task<ActionResult<IEnumerable<NationDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var nations = await _mediator.Send(new GetAllNationsQuery { WorldId = worldId });
            return Ok(nations);
        }

        // GET: /api/nations/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get nation by ID", Description = "Returns nation with its citizens")]
        [SwaggerResponse(200, "Nation found", typeof(NationDetailsDto))]
        [SwaggerResponse(404, "Nation not found")]
        public async Task<ActionResult<NationDetailsDto>> GetById(int id)
        {
            var nation = await _mediator.Send(new GetNationByIdQuery { Id = id });
            if (nation == null) return NotFound();
            return Ok(nation);
        }

        // POST: /api/nations
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new nation")]
        [SwaggerResponse(201, "Nation created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<NationDto>> Create([FromBody] NationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating nation: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateNationCommand { NationCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/nations/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update nation by ID")]
        [SwaggerResponse(200, "Nation updated", typeof(NationDto))]
        [SwaggerResponse(404, "Nation not found")]
        public async Task<ActionResult<NationDto>> Update(int id, [FromBody] NationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateNationCommand { Id = id, NationUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/nations/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete nation by ID", Description = "Fails with 400 if any character belongs to the nation")]
        [SwaggerResponse(204, "Nation deleted")]
        [SwaggerResponse(400, "Nation in use")]
        [SwaggerResponse(404, "Nation not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteNationCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
