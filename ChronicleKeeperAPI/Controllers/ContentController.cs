using ChronicleKeeper.Core.CQRS.Contents.Commands;
using ChronicleKeeper.Core.CQRS.Contents.Queries;
using ChronicleKeeper.Core.DTOs.Content;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/contents")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContentController> _logger;

        public ContentController(IMediator mediator, ILogger<ContentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/contents?worldId=1&type=Book
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get contents", Description = "Returns contents (Article/Book/Comic/Movie/Series), optionally filtered by world and/or type")]
        [SwaggerResponse(200, "List of contents", typeof(IEnumerable<ContentDto>))]
        public async Task<ActionResult<IEnumerable<ContentDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] string? type = null)
        {
            var contents = await _mediator.Send(new GetAllContentsQuery { WorldId = worldId, Type = type });
            return Ok(contents);
        }

        // GET: /api/contents/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get content by ID")]
        [SwaggerResponse(200, "Content found", typeof(ContentDetailsDto))]
        [SwaggerResponse(404, "Content not found")]
        public async Task<ActionResult<ContentDetailsDto>> GetById(int id)
        {
            var content = await _mediator.Send(new GetContentByIdQuery { Id = id });
            if (content == null) return NotFound();
            return Ok(content);
        }

        // POST: /api/contents
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new content", Description = "Type selects the concrete TPH subtype (Article/Book/Comic/Movie/Series) and cannot be changed afterward")]
        [SwaggerResponse(201, "Content created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ContentDto>> Create([FromBody] ContentCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating content: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateContentCommand { ContentCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/contents/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update content by ID", Description = "A content's concrete type cannot be changed")]
        [SwaggerResponse(200, "Content updated", typeof(ContentDto))]
        [SwaggerResponse(404, "Content not found")]
        public async Task<ActionResult<ContentDto>> Update(int id, [FromBody] ContentUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateContentCommand { Id = id, ContentUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/contents/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete content by ID")]
        [SwaggerResponse(204, "Content deleted")]
        [SwaggerResponse(404, "Content not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteContentCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
