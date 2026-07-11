using ChronicleKeeper.Core.CQRS.Chapters.Commands;
using ChronicleKeeper.Core.CQRS.Chapters.Queries;
using ChronicleKeeper.Core.DTOs.Chapter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/chapters")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ChapterController> _logger;

        public ChapterController(IMediator mediator, ILogger<ChapterController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/chapters?worldId=1&bookId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get chapters", Description = "Returns chapters, optionally filtered by world and/or book")]
        [SwaggerResponse(200, "List of chapters", typeof(IEnumerable<ChapterDto>))]
        public async Task<ActionResult<IEnumerable<ChapterDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? bookId = null)
        {
            var chapters = await _mediator.Send(new GetAllChaptersQuery { WorldId = worldId, BookId = bookId });
            return Ok(chapters);
        }

        // GET: /api/chapters/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get chapter by ID")]
        [SwaggerResponse(200, "Chapter found", typeof(ChapterDto))]
        [SwaggerResponse(404, "Chapter not found")]
        public async Task<ActionResult<ChapterDto>> GetById(int id)
        {
            var chapter = await _mediator.Send(new GetChapterByIdQuery { Id = id });
            if (chapter == null) return NotFound();
            return Ok(chapter);
        }

        // POST: /api/chapters
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new chapter", Description = "Chapter's world is derived from its book")]
        [SwaggerResponse(201, "Chapter created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ChapterDto>> Create([FromBody] ChapterCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating chapter: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateChapterCommand { ChapterCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/chapters/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update chapter by ID", Description = "A chapter's book cannot be changed")]
        [SwaggerResponse(200, "Chapter updated", typeof(ChapterDto))]
        [SwaggerResponse(404, "Chapter not found")]
        public async Task<ActionResult<ChapterDto>> Update(int id, [FromBody] ChapterUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateChapterCommand { Id = id, ChapterUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/chapters/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete chapter by ID")]
        [SwaggerResponse(204, "Chapter deleted")]
        [SwaggerResponse(404, "Chapter not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteChapterCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
