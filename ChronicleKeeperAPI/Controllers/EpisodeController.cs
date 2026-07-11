using ChronicleKeeper.Core.CQRS.Episodes.Commands;
using ChronicleKeeper.Core.CQRS.Episodes.Queries;
using ChronicleKeeper.Core.DTOs.Episode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/episodes")]
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EpisodeController> _logger;

        public EpisodeController(IMediator mediator, ILogger<EpisodeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/episodes?worldId=1&seriesId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get episodes", Description = "Returns episodes, optionally filtered by world and/or series")]
        [SwaggerResponse(200, "List of episodes", typeof(IEnumerable<EpisodeDto>))]
        public async Task<ActionResult<IEnumerable<EpisodeDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? seriesId = null)
        {
            var episodes = await _mediator.Send(new GetAllEpisodesQuery { WorldId = worldId, SeriesId = seriesId });
            return Ok(episodes);
        }

        // GET: /api/episodes/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get episode by ID")]
        [SwaggerResponse(200, "Episode found", typeof(EpisodeDto))]
        [SwaggerResponse(404, "Episode not found")]
        public async Task<ActionResult<EpisodeDto>> GetById(int id)
        {
            var episode = await _mediator.Send(new GetEpisodeByIdQuery { Id = id });
            if (episode == null) return NotFound();
            return Ok(episode);
        }

        // POST: /api/episodes
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new episode", Description = "Episode's world is derived from its series")]
        [SwaggerResponse(201, "Episode created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<EpisodeDto>> Create([FromBody] EpisodeCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating episode: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateEpisodeCommand { EpisodeCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/episodes/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update episode by ID", Description = "An episode's series cannot be changed")]
        [SwaggerResponse(200, "Episode updated", typeof(EpisodeDto))]
        [SwaggerResponse(404, "Episode not found")]
        public async Task<ActionResult<EpisodeDto>> Update(int id, [FromBody] EpisodeUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateEpisodeCommand { Id = id, EpisodeUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/episodes/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete episode by ID")]
        [SwaggerResponse(204, "Episode deleted")]
        [SwaggerResponse(404, "Episode not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEpisodeCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
