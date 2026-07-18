using ChronicleKeeper.Core.CQRS.Libraries.Commands;
using ChronicleKeeper.Core.CQRS.Libraries.Queries;
using ChronicleKeeper.Core.DTOs.Library;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/libraries")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(IMediator mediator, ILogger<LibraryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/libraries?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get libraries", Description = "Returns libraries, optionally filtered by world")]
        [SwaggerResponse(200, "List of libraries", typeof(IEnumerable<LibraryDto>))]
        public async Task<ActionResult<IEnumerable<LibraryDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var libraries = await _mediator.Send(new GetAllLibrariesQuery { WorldId = worldId });
            return Ok(libraries);
        }

        // GET: /api/libraries/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get library by ID")]
        [SwaggerResponse(200, "Library found", typeof(LibraryDetailsDto))]
        [SwaggerResponse(404, "Library not found")]
        public async Task<ActionResult<LibraryDetailsDto>> GetById(int id)
        {
            var library = await _mediator.Send(new GetLibraryByIdQuery { Id = id });
            if (library == null) return NotFound();
            return Ok(library);
        }

        // POST: /api/libraries
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new library")]
        [SwaggerResponse(201, "Library created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<LibraryDto>> Create([FromBody] LibraryCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating library: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateLibraryCommand { LibraryCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/libraries/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update library by ID")]
        [SwaggerResponse(200, "Library updated", typeof(LibraryDto))]
        [SwaggerResponse(404, "Library not found")]
        public async Task<ActionResult<LibraryDto>> Update(int id, [FromBody] LibraryUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateLibraryCommand { Id = id, LibraryUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/libraries/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete library by ID")]
        [SwaggerResponse(204, "Library deleted")]
        [SwaggerResponse(404, "Library not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteLibraryCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST/DELETE: /api/libraries/{id}/scholars/{characterId}
        [HttpPost("{id}/scholars/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Add a character as a scholar of the library")]
        public async Task<IActionResult> AddScholar(int id, int characterId)
        {
            await _mediator.Send(new AddLibraryScholarCommand { LibraryId = id, CharacterId = characterId });
            return NoContent();
        }

        [HttpDelete("{id}/scholars/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove a scholar from the library")]
        public async Task<IActionResult> RemoveScholar(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveLibraryScholarCommand { LibraryId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
