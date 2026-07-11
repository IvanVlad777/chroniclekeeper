using ChronicleKeeper.Core.CQRS.EducationSystems.Commands;
using ChronicleKeeper.Core.CQRS.EducationSystems.Queries;
using ChronicleKeeper.Core.DTOs.EducationSystem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/education-systems")]
    [ApiController]
    public class EducationSystemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EducationSystemController> _logger;

        public EducationSystemController(IMediator mediator, ILogger<EducationSystemController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/education-systems?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get education systems", Description = "Returns education systems, optionally filtered by world")]
        [SwaggerResponse(200, "List of education systems", typeof(IEnumerable<EducationSystemDto>))]
        public async Task<ActionResult<IEnumerable<EducationSystemDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var educationSystems = await _mediator.Send(new GetAllEducationSystemsQuery { WorldId = worldId });
            return Ok(educationSystems);
        }

        // GET: /api/education-systems/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get education system by ID", Description = "Returns education system with its schools and universities")]
        [SwaggerResponse(200, "Education system found", typeof(EducationSystemDetailsDto))]
        [SwaggerResponse(404, "Education system not found")]
        public async Task<ActionResult<EducationSystemDetailsDto>> GetById(int id)
        {
            var educationSystem = await _mediator.Send(new GetEducationSystemByIdQuery { Id = id });
            if (educationSystem == null) return NotFound();
            return Ok(educationSystem);
        }

        // POST: /api/education-systems
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new education system")]
        [SwaggerResponse(201, "Education system created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<EducationSystemDto>> Create([FromBody] EducationSystemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating education system: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateEducationSystemCommand { EducationSystemCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/education-systems/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update education system by ID")]
        [SwaggerResponse(200, "Education system updated", typeof(EducationSystemDto))]
        [SwaggerResponse(404, "Education system not found")]
        public async Task<ActionResult<EducationSystemDto>> Update(int id, [FromBody] EducationSystemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateEducationSystemCommand { Id = id, EducationSystemUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/education-systems/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete education system by ID", Description = "Cascades to its schools, trade schools and universities")]
        [SwaggerResponse(204, "Education system deleted")]
        [SwaggerResponse(404, "Education system not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEducationSystemCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
