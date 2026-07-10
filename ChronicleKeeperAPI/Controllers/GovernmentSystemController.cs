using ChronicleKeeper.Core.CQRS.GovernmentSystems.Commands;
using ChronicleKeeper.Core.CQRS.GovernmentSystems.Queries;
using ChronicleKeeper.Core.DTOs.GovernmentSystem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/governmentsystems")]
    [ApiController]
    public class GovernmentSystemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GovernmentSystemController> _logger;

        public GovernmentSystemController(IMediator mediator, ILogger<GovernmentSystemController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/governmentsystems?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get government systems", Description = "Returns government systems, optionally filtered by world")]
        [SwaggerResponse(200, "List of government systems", typeof(IEnumerable<GovernmentSystemDto>))]
        public async Task<ActionResult<IEnumerable<GovernmentSystemDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var systems = await _mediator.Send(new GetAllGovernmentSystemsQuery { WorldId = worldId });
            return Ok(systems);
        }

        // GET: /api/governmentsystems/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get government system by ID", Description = "Returns government system with its ideology and parties")]
        [SwaggerResponse(200, "Government system found", typeof(GovernmentSystemDetailsDto))]
        [SwaggerResponse(404, "Government system not found")]
        public async Task<ActionResult<GovernmentSystemDetailsDto>> GetById(int id)
        {
            var system = await _mediator.Send(new GetGovernmentSystemByIdQuery { Id = id });
            if (system == null) return NotFound();
            return Ok(system);
        }

        // POST: /api/governmentsystems
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new government system")]
        [SwaggerResponse(201, "Government system created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<GovernmentSystemDto>> Create([FromBody] GovernmentSystemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating government system: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateGovernmentSystemCommand { GovernmentSystemCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/governmentsystems/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update government system by ID")]
        [SwaggerResponse(200, "Government system updated", typeof(GovernmentSystemDto))]
        [SwaggerResponse(404, "Government system not found")]
        public async Task<ActionResult<GovernmentSystemDto>> Update(int id, [FromBody] GovernmentSystemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateGovernmentSystemCommand { Id = id, GovernmentSystemUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/governmentsystems/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete government system by ID", Description = "Fails with 400 if any political party uses the government system")]
        [SwaggerResponse(204, "Government system deleted")]
        [SwaggerResponse(400, "Government system in use")]
        [SwaggerResponse(404, "Government system not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGovernmentSystemCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
