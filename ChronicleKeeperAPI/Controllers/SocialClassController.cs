using ChronicleKeeper.Core.CQRS.SocialClasses.Commands;
using ChronicleKeeper.Core.CQRS.SocialClasses.Queries;
using ChronicleKeeper.Core.DTOs.SocialClass;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/social-classes")]
    [ApiController]
    public class SocialClassController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SocialClassController> _logger;

        public SocialClassController(IMediator mediator, ILogger<SocialClassController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/social-classes?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get social classes", Description = "Returns social classes, optionally filtered by world")]
        [SwaggerResponse(200, "List of social classes", typeof(IEnumerable<SocialClassDto>))]
        public async Task<ActionResult<IEnumerable<SocialClassDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var socialClasses = await _mediator.Send(new GetAllSocialClassesQuery { WorldId = worldId });
            return Ok(socialClasses);
        }

        // GET: /api/social-classes/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get social class by ID", Description = "Returns social class with its members")]
        [SwaggerResponse(200, "Social class found", typeof(SocialClassDetailsDto))]
        [SwaggerResponse(404, "Social class not found")]
        public async Task<ActionResult<SocialClassDetailsDto>> GetById(int id)
        {
            var socialClass = await _mediator.Send(new GetSocialClassByIdQuery { Id = id });
            if (socialClass == null) return NotFound();
            return Ok(socialClass);
        }

        // POST: /api/social-classes
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new social class")]
        [SwaggerResponse(201, "Social class created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<SocialClassDto>> Create([FromBody] SocialClassCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating social class: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateSocialClassCommand { SocialClassCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/social-classes/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update social class by ID")]
        [SwaggerResponse(200, "Social class updated", typeof(SocialClassDto))]
        [SwaggerResponse(404, "Social class not found")]
        public async Task<ActionResult<SocialClassDto>> Update(int id, [FromBody] SocialClassUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSocialClassCommand { Id = id, SocialClassUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/social-classes/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete social class by ID", Description = "Fails with 400 if any character uses the social class")]
        [SwaggerResponse(204, "Social class deleted")]
        [SwaggerResponse(400, "Social class in use")]
        [SwaggerResponse(404, "Social class not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSocialClassCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
