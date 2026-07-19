using ChronicleKeeper.Core.CQRS.UniversityMajors.Commands;
using ChronicleKeeper.Core.CQRS.UniversityMajors.Queries;
using ChronicleKeeper.Core.DTOs.UniversityMajor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/university-majors")]
    [ApiController]
    public class UniversityMajorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UniversityMajorController> _logger;

        public UniversityMajorController(IMediator mediator, ILogger<UniversityMajorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/university-majors?worldId=1&universityId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get university majors", Description = "Returns university majors, optionally filtered by world and/or university")]
        [SwaggerResponse(200, "List of university majors", typeof(IEnumerable<UniversityMajorDto>))]
        public async Task<ActionResult<IEnumerable<UniversityMajorDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? universityId = null)
        {
            var majors = await _mediator.Send(new GetAllUniversityMajorsQuery { WorldId = worldId, UniversityId = universityId });
            return Ok(majors);
        }

        // GET: /api/university-majors/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get university major by ID")]
        [SwaggerResponse(200, "University major found", typeof(UniversityMajorDetailsDto))]
        [SwaggerResponse(404, "University major not found")]
        public async Task<ActionResult<UniversityMajorDetailsDto>> GetById(int id)
        {
            var major = await _mediator.Send(new GetUniversityMajorByIdQuery { Id = id });
            if (major == null) return NotFound();
            return Ok(major);
        }

        // POST: /api/university-majors
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new university major", Description = "Major's world is derived from its university")]
        [SwaggerResponse(201, "University major created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<UniversityMajorDto>> Create([FromBody] UniversityMajorCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating university major: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateUniversityMajorCommand { UniversityMajorCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/university-majors/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update university major by ID", Description = "A major's university cannot be changed")]
        [SwaggerResponse(200, "University major updated", typeof(UniversityMajorDto))]
        [SwaggerResponse(404, "University major not found")]
        public async Task<ActionResult<UniversityMajorDto>> Update(int id, [FromBody] UniversityMajorUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateUniversityMajorCommand { Id = id, UniversityMajorUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/university-majors/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete university major by ID")]
        [SwaggerResponse(204, "University major deleted")]
        [SwaggerResponse(404, "University major not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUniversityMajorCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST/DELETE: /api/university-majors/{id}/professors/{characterId}
        [HttpPost("{id}/professors/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a professor (character) to this major")]
        [SwaggerResponse(204, "Professor linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddProfessor(int id, int characterId)
        {
            await _mediator.Send(new AddUniversityMajorProfessorCommand { UniversityMajorId = id, CharacterId = characterId });
            return NoContent();
        }

        [HttpDelete("{id}/professors/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a professor from this major")]
        [SwaggerResponse(204, "Professor unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveProfessor(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveUniversityMajorProfessorCommand { UniversityMajorId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST/DELETE: /api/university-majors/{id}/students/{characterId}
        [HttpPost("{id}/students/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a student (character) to this major")]
        [SwaggerResponse(204, "Student linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddStudent(int id, int characterId)
        {
            await _mediator.Send(new AddUniversityMajorStudentCommand { UniversityMajorId = id, CharacterId = characterId });
            return NoContent();
        }

        [HttpDelete("{id}/students/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a student from this major")]
        [SwaggerResponse(204, "Student unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveStudent(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveUniversityMajorStudentCommand { UniversityMajorId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
