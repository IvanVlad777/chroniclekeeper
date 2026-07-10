using ChronicleKeeper.Core.CQRS.PoliticalIdeologies.Commands;
using ChronicleKeeper.Core.CQRS.PoliticalIdeologies.Queries;
using ChronicleKeeper.Core.DTOs.PoliticalIdeology;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/politicalideologies")]
    [ApiController]
    public class PoliticalIdeologyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PoliticalIdeologyController> _logger;

        public PoliticalIdeologyController(IMediator mediator, ILogger<PoliticalIdeologyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/politicalideologies?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get political ideologies", Description = "Returns political ideologies, optionally filtered by world")]
        [SwaggerResponse(200, "List of political ideologies", typeof(IEnumerable<PoliticalIdeologyDto>))]
        public async Task<ActionResult<IEnumerable<PoliticalIdeologyDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var ideologies = await _mediator.Send(new GetAllPoliticalIdeologiesQuery { WorldId = worldId });
            return Ok(ideologies);
        }

        // GET: /api/politicalideologies/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get political ideology by ID", Description = "Returns political ideology with affiliated parties/government systems")]
        [SwaggerResponse(200, "Political ideology found", typeof(PoliticalIdeologyDetailsDto))]
        [SwaggerResponse(404, "Political ideology not found")]
        public async Task<ActionResult<PoliticalIdeologyDetailsDto>> GetById(int id)
        {
            var ideology = await _mediator.Send(new GetPoliticalIdeologyByIdQuery { Id = id });
            if (ideology == null) return NotFound();
            return Ok(ideology);
        }

        // POST: /api/politicalideologies
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new political ideology")]
        [SwaggerResponse(201, "Political ideology created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<PoliticalIdeologyDto>> Create([FromBody] PoliticalIdeologyCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating political ideology: {Name}", dto.Name);
            var result = await _mediator.Send(new CreatePoliticalIdeologyCommand { PoliticalIdeologyCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/politicalideologies/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update political ideology by ID")]
        [SwaggerResponse(200, "Political ideology updated", typeof(PoliticalIdeologyDto))]
        [SwaggerResponse(404, "Political ideology not found")]
        public async Task<ActionResult<PoliticalIdeologyDto>> Update(int id, [FromBody] PoliticalIdeologyUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdatePoliticalIdeologyCommand { Id = id, PoliticalIdeologyUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/politicalideologies/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete political ideology by ID", Description = "Fails with 400 if any party or government system uses the ideology")]
        [SwaggerResponse(204, "Political ideology deleted")]
        [SwaggerResponse(400, "Political ideology in use")]
        [SwaggerResponse(404, "Political ideology not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePoliticalIdeologyCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
