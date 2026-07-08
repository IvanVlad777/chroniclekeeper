using ChronicleKeeper.Core.CQRS.Notes;
using ChronicleKeeper.Core.DTOs.Note;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NoteController> _logger;

        public NoteController(IMediator mediator, ILogger<NoteController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/notes?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get notes", Description = "Returns notes, optionally filtered by world")]
        [SwaggerResponse(200, "List of notes", typeof(IEnumerable<NoteDto>))]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var notes = await _mediator.Send(new GetAllNotesQuery { WorldId = worldId });
            return Ok(notes);
        }

        // GET: /api/notes/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get note by ID")]
        [SwaggerResponse(200, "Note found", typeof(NoteDto))]
        [SwaggerResponse(404, "Note not found")]
        public async Task<ActionResult<NoteDto>> GetById(int id)
        {
            var note = await _mediator.Send(new GetNoteByIdQuery { Id = id });
            if (note == null) return NotFound();
            return Ok(note);
        }

        // POST: /api/notes
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new note")]
        [SwaggerResponse(201, "Note created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<NoteDto>> Create([FromBody] NoteCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating note: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateNoteCommand { NoteCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/notes/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update note by ID")]
        [SwaggerResponse(200, "Note updated", typeof(NoteDto))]
        [SwaggerResponse(404, "Note not found")]
        public async Task<ActionResult<NoteDto>> Update(int id, [FromBody] NoteUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateNoteCommand { Id = id, NoteUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/notes/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete note by ID")]
        [SwaggerResponse(204, "Note deleted")]
        [SwaggerResponse(404, "Note not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteNoteCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
