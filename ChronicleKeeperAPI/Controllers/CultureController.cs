using ChronicleKeeper.Core.CQRS.Cultures.Commands;
using ChronicleKeeper.Core.CQRS.Cultures.Queries;
using ChronicleKeeper.Core.DTOs.Culture;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/cultures")]
    [ApiController]
    public class CultureController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CultureController> _logger;

        public CultureController(IMediator mediator, ILogger<CultureController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/cultures?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get cultures", Description = "Returns cultures, optionally filtered by world")]
        [SwaggerResponse(200, "List of cultures", typeof(IEnumerable<CultureDto>))]
        public async Task<ActionResult<IEnumerable<CultureDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var cultures = await _mediator.Send(new GetAllCulturesQuery { WorldId = worldId });
            return Ok(cultures);
        }

        // GET: /api/cultures/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get culture by ID", Description = "Returns culture with its language and religion")]
        [SwaggerResponse(200, "Culture found", typeof(CultureDetailsDto))]
        [SwaggerResponse(404, "Culture not found")]
        public async Task<ActionResult<CultureDetailsDto>> GetById(int id)
        {
            var culture = await _mediator.Send(new GetCultureByIdQuery { Id = id });
            if (culture == null) return NotFound();
            return Ok(culture);
        }

        // POST: /api/cultures
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new culture")]
        [SwaggerResponse(201, "Culture created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<CultureDto>> Create([FromBody] CultureCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating culture: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateCultureCommand { CultureCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/cultures/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update culture by ID")]
        [SwaggerResponse(200, "Culture updated", typeof(CultureDto))]
        [SwaggerResponse(404, "Culture not found")]
        public async Task<ActionResult<CultureDto>> Update(int id, [FromBody] CultureUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateCultureCommand { Id = id, CultureUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/cultures/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete culture by ID")]
        [SwaggerResponse(204, "Culture deleted")]
        [SwaggerResponse(404, "Culture not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCultureCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
