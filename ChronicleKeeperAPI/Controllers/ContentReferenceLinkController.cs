using ChronicleKeeper.Core.CQRS.ContentReferenceLinks.Commands;
using ChronicleKeeper.Core.CQRS.ContentReferenceLinks.Queries;
using ChronicleKeeper.Core.DTOs.ContentReferenceLink;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    /// <summary>Endpoints for the <c>Reference</c> entity — routed as "references"; named
    /// ContentReferenceLink internally to avoid clashing with the generic {Id,Name} ReferenceDto.</summary>
    [Route("api/references")]
    [ApiController]
    public class ContentReferenceLinkController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContentReferenceLinkController> _logger;

        public ContentReferenceLinkController(IMediator mediator, ILogger<ContentReferenceLinkController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/references?contentId=1&chapterId=2&episodeId=3&characterId=4&locationId=5&factionId=6&nationId=7
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get content reference links", Description = "Returns Reference rows linking narrative units (Content/Chapter/Episode) to world entities (Character/Location/Faction/Nation), optionally filtered by any of the 7 FKs")]
        [SwaggerResponse(200, "List of content reference links", typeof(IEnumerable<ContentReferenceLinkDto>))]
        public async Task<ActionResult<IEnumerable<ContentReferenceLinkDto>>> GetAll(
            [FromQuery] int? contentId = null,
            [FromQuery] int? chapterId = null,
            [FromQuery] int? episodeId = null,
            [FromQuery] int? characterId = null,
            [FromQuery] int? locationId = null,
            [FromQuery] int? factionId = null,
            [FromQuery] int? nationId = null)
        {
            var references = await _mediator.Send(new GetAllContentReferenceLinksQuery
            {
                ContentId = contentId,
                ChapterId = chapterId,
                EpisodeId = episodeId,
                CharacterId = characterId,
                LocationId = locationId,
                FactionId = factionId,
                NationId = nationId
            });
            return Ok(references);
        }

        // GET: /api/references/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get content reference link by ID")]
        [SwaggerResponse(200, "Content reference link found", typeof(ContentReferenceLinkDto))]
        [SwaggerResponse(404, "Content reference link not found")]
        public async Task<ActionResult<ContentReferenceLinkDto>> GetById(int id)
        {
            var reference = await _mediator.Send(new GetContentReferenceLinkByIdQuery { Id = id });
            if (reference == null) return NotFound();
            return Ok(reference);
        }

        // POST: /api/references
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new content reference link")]
        [SwaggerResponse(201, "Content reference link created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ContentReferenceLinkDto>> Create([FromBody] ContentReferenceLinkCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating content reference link");
            var result = await _mediator.Send(new CreateContentReferenceLinkCommand { ContentReferenceLinkCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/references/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update content reference link by ID")]
        [SwaggerResponse(200, "Content reference link updated", typeof(ContentReferenceLinkDto))]
        [SwaggerResponse(404, "Content reference link not found")]
        public async Task<ActionResult<ContentReferenceLinkDto>> Update(int id, [FromBody] ContentReferenceLinkUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateContentReferenceLinkCommand { Id = id, ContentReferenceLinkUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/references/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete content reference link by ID")]
        [SwaggerResponse(204, "Content reference link deleted")]
        [SwaggerResponse(404, "Content reference link not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteContentReferenceLinkCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
