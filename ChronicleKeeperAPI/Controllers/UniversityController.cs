using ChronicleKeeper.Core.CQRS.Universities.Commands;
using ChronicleKeeper.Core.CQRS.Universities.Queries;
using ChronicleKeeper.Core.DTOs.University;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/universities")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UniversityController> _logger;

        public UniversityController(IMediator mediator, ILogger<UniversityController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/universities?worldId=1&educationSystemId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get universities", Description = "Returns universities, optionally filtered by world and/or education system")]
        [SwaggerResponse(200, "List of universities", typeof(IEnumerable<UniversityDto>))]
        public async Task<ActionResult<IEnumerable<UniversityDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? educationSystemId = null)
        {
            var universities = await _mediator.Send(new GetAllUniversitiesQuery { WorldId = worldId, EducationSystemId = educationSystemId });
            return Ok(universities);
        }

        // GET: /api/universities/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get university by ID", Description = "Returns university with its majors and alumni")]
        [SwaggerResponse(200, "University found", typeof(UniversityDetailsDto))]
        [SwaggerResponse(404, "University not found")]
        public async Task<ActionResult<UniversityDetailsDto>> GetById(int id)
        {
            var university = await _mediator.Send(new GetUniversityByIdQuery { Id = id });
            if (university == null) return NotFound();
            return Ok(university);
        }

        // POST: /api/universities
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new university", Description = "University's world is derived from its education system")]
        [SwaggerResponse(201, "University created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<UniversityDto>> Create([FromBody] UniversityCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating university: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateUniversityCommand { UniversityCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/universities/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update university by ID", Description = "A university's education system cannot be changed")]
        [SwaggerResponse(200, "University updated", typeof(UniversityDto))]
        [SwaggerResponse(404, "University not found")]
        public async Task<ActionResult<UniversityDto>> Update(int id, [FromBody] UniversityUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateUniversityCommand { Id = id, UniversityUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/universities/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete university by ID", Description = "Fails with 400 if any education record or library references the university")]
        [SwaggerResponse(204, "University deleted")]
        [SwaggerResponse(400, "University in use")]
        [SwaggerResponse(404, "University not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUniversityCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST/DELETE: /api/universities/{id}/students/{characterId}
        [HttpPost("{id}/students/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Enrol a character as a student of the university")]
        public async Task<IActionResult> AddStudent(int id, int characterId)
        {
            await _mediator.Send(new AddUniversityStudentCommand { UniversityId = id, CharacterId = characterId });
            return NoContent();
        }

        [HttpDelete("{id}/students/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove a student from the university")]
        public async Task<IActionResult> RemoveStudent(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveUniversityStudentCommand { UniversityId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST/DELETE: /api/universities/{id}/professors/{characterId}
        [HttpPost("{id}/professors/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Add a character as a professor of the university")]
        public async Task<IActionResult> AddProfessor(int id, int characterId)
        {
            await _mediator.Send(new AddUniversityProfessorCommand { UniversityId = id, CharacterId = characterId });
            return NoContent();
        }

        [HttpDelete("{id}/professors/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove a professor from the university")]
        public async Task<IActionResult> RemoveProfessor(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveUniversityProfessorCommand { UniversityId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
