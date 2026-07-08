using ChronicleKeeper.Core.CQRS.Worlds.Commands;
using ChronicleKeeper.Core.CQRS.Worlds.Queries;
using ChronicleKeeper.Core.DTOs.World;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/worlds")]
    [ApiController]
    public class WorldController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WorldController> _logger;

        public WorldController(IMediator mediator, ILogger<WorldController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        private string? RequesterId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        private bool RequesterIsAdmin => User.IsInRole("Admin") || User.IsInRole("SuperAdmin");

        // GET: /api/worlds  (samo site admini — popis svih svjetova svih korisnika)
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Get all worlds (admin)", Description = "Returns all worlds of all users — site administration only")]
        [SwaggerResponse(200, "List of worlds", typeof(IEnumerable<WorldDto>))]
        public async Task<ActionResult<IEnumerable<WorldDto>>> GetAll()
        {
            _logger.LogInformation("API: Fetching all worlds (admin)");
            var worlds = await _mediator.Send(new GetAllWorldsQuery());
            return Ok(worlds);
        }

        // GET: /api/worlds/mine
        [HttpGet("mine")]
        [Authorize]
        [SwaggerOperation(Summary = "Get my worlds", Description = "Returns worlds owned by the current user")]
        [SwaggerResponse(200, "List of worlds", typeof(IEnumerable<WorldDto>))]
        public async Task<ActionResult<IEnumerable<WorldDto>>> GetMine()
        {
            var ownerId = RequesterId;
            if (ownerId == null) return Unauthorized();

            var worlds = await _mediator.Send(new GetAllWorldsQuery { OwnerId = ownerId });
            return Ok(worlds);
        }

        // GET: /api/worlds/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get world by ID", Description = "Returns a single world")]
        [SwaggerResponse(200, "World found", typeof(WorldDto))]
        [SwaggerResponse(404, "World not found")]
        public async Task<ActionResult<WorldDto>> GetById(int id)
        {
            var world = await _mediator.Send(new GetWorldByIdQuery { Id = id });
            if (world == null)
            {
                _logger.LogWarning("API: World with ID {Id} not found", id);
                return NotFound();
            }
            return Ok(world);
        }

        // POST: /api/worlds
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new world", Description = "Creates a new world owned by the current user")]
        [SwaggerResponse(201, "World created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<WorldDto>> Create([FromBody] WorldCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerId = RequesterId;
            if (ownerId == null) return Unauthorized();

            _logger.LogInformation("API: Creating world: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateWorldCommand { WorldCreateDto = dto, OwnerId = ownerId });

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/worlds/{id}  (vlasnik ili site admin — provjera u handleru)
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update world by ID", Description = "Updates an existing world — owner or site admin only")]
        [SwaggerResponse(200, "World updated", typeof(WorldDto))]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(403, "Not the owner")]
        [SwaggerResponse(404, "World not found")]
        public async Task<ActionResult<WorldDto>> Update(int id, [FromBody] WorldUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var requesterId = RequesterId;
            if (requesterId == null) return Unauthorized();

            var result = await _mediator.Send(new UpdateWorldCommand
            {
                Id = id,
                WorldUpdateDto = dto,
                RequesterId = requesterId,
                RequesterIsAdmin = RequesterIsAdmin
            });
            return Ok(result);
        }

        // DELETE: /api/worlds/{id}  (vlasnik ili site admin — provjera u handleru)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete world by ID", Description = "Deletes a world and ALL of its lore data (characters, locations, factions, ...) — owner or site admin only")]
        [SwaggerResponse(204, "World deleted")]
        [SwaggerResponse(403, "Not the owner")]
        [SwaggerResponse(404, "World not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var requesterId = RequesterId;
            if (requesterId == null) return Unauthorized();

            _logger.LogInformation("API: Deleting world with ID {Id}", id);
            var result = await _mediator.Send(new DeleteWorldCommand
            {
                Id = id,
                RequesterId = requesterId,
                RequesterIsAdmin = RequesterIsAdmin
            });

            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
