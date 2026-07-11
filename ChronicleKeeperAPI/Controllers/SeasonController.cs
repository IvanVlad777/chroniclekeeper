using ChronicleKeeper.Core.CQRS.Seasons.Commands;
using ChronicleKeeper.Core.CQRS.Seasons.Queries;
using ChronicleKeeper.Core.DTOs.Season;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/seasons")]
    [ApiController]
    public class SeasonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SeasonController> _logger;

        public SeasonController(IMediator mediator, ILogger<SeasonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/seasons?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get seasons", Description = "Returns reusable seasons, optionally filtered by world")]
        [SwaggerResponse(200, "List of seasons", typeof(IEnumerable<SeasonDto>))]
        public async Task<ActionResult<IEnumerable<SeasonDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var seasons = await _mediator.Send(new GetAllSeasonsQuery { WorldId = worldId });
            return Ok(seasons);
        }

        // GET: /api/seasons/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get season by ID")]
        [SwaggerResponse(200, "Season found", typeof(SeasonDto))]
        [SwaggerResponse(404, "Season not found")]
        public async Task<ActionResult<SeasonDto>> GetById(int id)
        {
            var season = await _mediator.Send(new GetSeasonByIdQuery { Id = id });
            if (season == null) return NotFound();
            return Ok(season);
        }

        // POST: /api/seasons
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new season")]
        [SwaggerResponse(201, "Season created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<SeasonDto>> Create([FromBody] SeasonCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating season: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateSeasonCommand { SeasonCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/seasons/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update season by ID")]
        [SwaggerResponse(200, "Season updated", typeof(SeasonDto))]
        [SwaggerResponse(404, "Season not found")]
        public async Task<ActionResult<SeasonDto>> Update(int id, [FromBody] SeasonUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSeasonCommand { Id = id, SeasonUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/seasons/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete season by ID")]
        [SwaggerResponse(204, "Season deleted")]
        [SwaggerResponse(404, "Season not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSeasonCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
