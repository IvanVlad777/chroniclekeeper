using ChronicleKeeper.Core.CQRS.Religions.Commands;
using ChronicleKeeper.Core.CQRS.Religions.Queries;
using ChronicleKeeper.Core.DTOs.Religion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/religions")]
    [ApiController]
    public class ReligionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReligionController> _logger;

        public ReligionController(IMediator mediator, ILogger<ReligionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/religions?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get religions", Description = "Returns religions, optionally filtered by world")]
        [SwaggerResponse(200, "List of religions", typeof(IEnumerable<ReligionDto>))]
        public async Task<ActionResult<IEnumerable<ReligionDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var religions = await _mediator.Send(new GetAllReligionsQuery { WorldId = worldId });
            return Ok(religions);
        }

        // GET: /api/religions/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get religion by ID", Description = "Returns religion with its followers")]
        [SwaggerResponse(200, "Religion found", typeof(ReligionDetailsDto))]
        [SwaggerResponse(404, "Religion not found")]
        public async Task<ActionResult<ReligionDetailsDto>> GetById(int id)
        {
            var religion = await _mediator.Send(new GetReligionByIdQuery { Id = id });
            if (religion == null) return NotFound();
            return Ok(religion);
        }

        // POST: /api/religions
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new religion")]
        [SwaggerResponse(201, "Religion created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ReligionDto>> Create([FromBody] ReligionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating religion: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateReligionCommand { ReligionCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/religions/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update religion by ID")]
        [SwaggerResponse(200, "Religion updated", typeof(ReligionDto))]
        [SwaggerResponse(404, "Religion not found")]
        public async Task<ActionResult<ReligionDto>> Update(int id, [FromBody] ReligionUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateReligionCommand { Id = id, ReligionUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/religions/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete religion by ID", Description = "Fails with 400 if any character follows the religion")]
        [SwaggerResponse(204, "Religion deleted")]
        [SwaggerResponse(400, "Religion in use")]
        [SwaggerResponse(404, "Religion not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteReligionCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
