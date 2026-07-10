using ChronicleKeeper.Core.CQRS.LegalSystems.Commands;
using ChronicleKeeper.Core.CQRS.LegalSystems.Queries;
using ChronicleKeeper.Core.DTOs.LegalSystem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/legalsystems")]
    [ApiController]
    public class LegalSystemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LegalSystemController> _logger;

        public LegalSystemController(IMediator mediator, ILogger<LegalSystemController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/legalsystems?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get legal systems", Description = "Returns legal systems, optionally filtered by world")]
        [SwaggerResponse(200, "List of legal systems", typeof(IEnumerable<LegalSystemDto>))]
        public async Task<ActionResult<IEnumerable<LegalSystemDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var legalSystems = await _mediator.Send(new GetAllLegalSystemsQuery { WorldId = worldId });
            return Ok(legalSystems);
        }

        // GET: /api/legalsystems/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get legal system by ID")]
        [SwaggerResponse(200, "Legal system found", typeof(LegalSystemDetailsDto))]
        [SwaggerResponse(404, "Legal system not found")]
        public async Task<ActionResult<LegalSystemDetailsDto>> GetById(int id)
        {
            var legalSystem = await _mediator.Send(new GetLegalSystemByIdQuery { Id = id });
            if (legalSystem == null) return NotFound();
            return Ok(legalSystem);
        }

        // POST: /api/legalsystems
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new legal system")]
        [SwaggerResponse(201, "Legal system created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<LegalSystemDto>> Create([FromBody] LegalSystemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating legal system: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateLegalSystemCommand { LegalSystemCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/legalsystems/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update legal system by ID")]
        [SwaggerResponse(200, "Legal system updated", typeof(LegalSystemDto))]
        [SwaggerResponse(404, "Legal system not found")]
        public async Task<ActionResult<LegalSystemDto>> Update(int id, [FromBody] LegalSystemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateLegalSystemCommand { Id = id, LegalSystemUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/legalsystems/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete legal system by ID")]
        [SwaggerResponse(204, "Legal system deleted")]
        [SwaggerResponse(404, "Legal system not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteLegalSystemCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
