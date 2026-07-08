using ChronicleKeeper.Core.CQRS.Tags;
using ChronicleKeeper.Core.DTOs.Tag;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TagController> _logger;

        public TagController(IMediator mediator, ILogger<TagController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/tags?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get tags", Description = "Returns tags, optionally filtered by world")]
        [SwaggerResponse(200, "List of tags", typeof(IEnumerable<TagDto>))]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var tags = await _mediator.Send(new GetAllTagsQuery { WorldId = worldId });
            return Ok(tags);
        }

        // GET: /api/tags/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get tag by ID")]
        [SwaggerResponse(200, "Tag found", typeof(TagDto))]
        [SwaggerResponse(404, "Tag not found")]
        public async Task<ActionResult<TagDto>> GetById(int id)
        {
            var tag = await _mediator.Send(new GetTagByIdQuery { Id = id });
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        // POST: /api/tags
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new tag", Description = "Tag names are unique per world")]
        [SwaggerResponse(201, "Tag created")]
        [SwaggerResponse(400, "Invalid input / duplicate name")]
        public async Task<ActionResult<TagDto>> Create([FromBody] TagCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating tag: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateTagCommand { TagCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/tags/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update tag by ID")]
        [SwaggerResponse(200, "Tag updated", typeof(TagDto))]
        [SwaggerResponse(400, "Duplicate name")]
        [SwaggerResponse(404, "Tag not found")]
        public async Task<ActionResult<TagDto>> Update(int id, [FromBody] TagUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateTagCommand { Id = id, TagUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/tags/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete tag by ID", Description = "All attachments are removed automatically")]
        [SwaggerResponse(204, "Tag deleted")]
        [SwaggerResponse(404, "Tag not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTagCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/tags/{id}/attachments
        [HttpPost("{id}/attachments")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Attach tag to an entity", Description = "TargetType: Character | Location | Faction; target must be in the tag's world")]
        [SwaggerResponse(204, "Tag attached")]
        [SwaggerResponse(400, "Invalid target / already attached")]
        [SwaggerResponse(404, "Tag not found")]
        public async Task<IActionResult> Attach(int id, [FromBody] TagAttachDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _mediator.Send(new AttachTagCommand { TagId = id, AttachDto = dto });
            return NoContent();
        }

        // DELETE: /api/tags/{id}/attachments/{targetType}/{targetId}
        [HttpDelete("{id}/attachments/{targetType}/{targetId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Detach tag from an entity")]
        [SwaggerResponse(204, "Tag detached")]
        [SwaggerResponse(404, "Attachment not found")]
        public async Task<IActionResult> Detach(int id, TagTargetType targetType, int targetId)
        {
            var result = await _mediator.Send(new DetachTagCommand { TagId = id, TargetType = targetType, TargetId = targetId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
