using ChronicleKeeper.Core.CQRS.SocialHierarchies.Commands;
using ChronicleKeeper.Core.CQRS.SocialHierarchies.Queries;
using ChronicleKeeper.Core.DTOs.SocialHierarchy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/social-hierarchies")]
    [ApiController]
    public class SocialHierarchyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SocialHierarchyController> _logger;

        public SocialHierarchyController(IMediator mediator, ILogger<SocialHierarchyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/social-hierarchies?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get social hierarchies", Description = "Returns social hierarchies, optionally filtered by world")]
        [SwaggerResponse(200, "List of social hierarchies", typeof(IEnumerable<SocialHierarchyDto>))]
        public async Task<ActionResult<IEnumerable<SocialHierarchyDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var hierarchies = await _mediator.Send(new GetAllSocialHierarchiesQuery { WorldId = worldId });
            return Ok(hierarchies);
        }

        // GET: /api/social-hierarchies/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get social hierarchy by ID", Description = "Returns hierarchy with its classes and nations")]
        [SwaggerResponse(200, "Social hierarchy found", typeof(SocialHierarchyDetailsDto))]
        [SwaggerResponse(404, "Social hierarchy not found")]
        public async Task<ActionResult<SocialHierarchyDetailsDto>> GetById(int id)
        {
            var hierarchy = await _mediator.Send(new GetSocialHierarchyByIdQuery { Id = id });
            if (hierarchy == null) return NotFound();
            return Ok(hierarchy);
        }

        // POST: /api/social-hierarchies
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new social hierarchy")]
        [SwaggerResponse(201, "Social hierarchy created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<SocialHierarchyDto>> Create([FromBody] SocialHierarchyCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating social hierarchy: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateSocialHierarchyCommand { SocialHierarchyCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/social-hierarchies/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update social hierarchy by ID")]
        [SwaggerResponse(200, "Social hierarchy updated", typeof(SocialHierarchyDto))]
        [SwaggerResponse(404, "Social hierarchy not found")]
        public async Task<ActionResult<SocialHierarchyDto>> Update(int id, [FromBody] SocialHierarchyUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSocialHierarchyCommand { Id = id, SocialHierarchyUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/social-hierarchies/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete social hierarchy by ID")]
        [SwaggerResponse(204, "Social hierarchy deleted")]
        [SwaggerResponse(404, "Social hierarchy not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSocialHierarchyCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
