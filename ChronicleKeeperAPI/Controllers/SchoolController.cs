using ChronicleKeeper.Core.CQRS.Schools.Commands;
using ChronicleKeeper.Core.CQRS.Schools.Queries;
using ChronicleKeeper.Core.DTOs.School;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/schools")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SchoolController> _logger;

        public SchoolController(IMediator mediator, ILogger<SchoolController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/schools?worldId=1&educationSystemId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get schools", Description = "Returns schools (incl. trade schools), optionally filtered by world and/or education system")]
        [SwaggerResponse(200, "List of schools", typeof(IEnumerable<SchoolDto>))]
        public async Task<ActionResult<IEnumerable<SchoolDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? educationSystemId = null)
        {
            var schools = await _mediator.Send(new GetAllSchoolsQuery { WorldId = worldId, EducationSystemId = educationSystemId });
            return Ok(schools);
        }

        // GET: /api/schools/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get school by ID", Description = "Returns school with its subjects and alumni")]
        [SwaggerResponse(200, "School found", typeof(SchoolDetailsDto))]
        [SwaggerResponse(404, "School not found")]
        public async Task<ActionResult<SchoolDetailsDto>> GetById(int id)
        {
            var school = await _mediator.Send(new GetSchoolByIdQuery { Id = id });
            if (school == null) return NotFound();
            return Ok(school);
        }

        // POST: /api/schools
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new school", Description = "School's world is derived from its education system")]
        [SwaggerResponse(201, "School created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<SchoolDto>> Create([FromBody] SchoolCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating school: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateSchoolCommand { SchoolCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/schools/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update school by ID", Description = "A school's education system cannot be changed")]
        [SwaggerResponse(200, "School updated", typeof(SchoolDto))]
        [SwaggerResponse(404, "School not found")]
        public async Task<ActionResult<SchoolDto>> Update(int id, [FromBody] SchoolUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSchoolCommand { Id = id, SchoolUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/schools/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete school by ID", Description = "Fails with 400 if any education record uses the school")]
        [SwaggerResponse(204, "School deleted")]
        [SwaggerResponse(400, "School in use")]
        [SwaggerResponse(404, "School not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSchoolCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST/DELETE: /api/schools/{id}/students/{characterId}
        [HttpPost("{id}/students/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Enrol a character as a student of the school")]
        public async Task<IActionResult> AddStudent(int id, int characterId)
        {
            await _mediator.Send(new AddSchoolStudentCommand { SchoolId = id, CharacterId = characterId });
            return NoContent();
        }

        [HttpDelete("{id}/students/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove a student from the school")]
        public async Task<IActionResult> RemoveStudent(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveSchoolStudentCommand { SchoolId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST/DELETE: /api/schools/{id}/teachers/{characterId}
        [HttpPost("{id}/teachers/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Add a character as a teacher of the school")]
        public async Task<IActionResult> AddTeacher(int id, int characterId)
        {
            await _mediator.Send(new AddSchoolTeacherCommand { SchoolId = id, CharacterId = characterId });
            return NoContent();
        }

        [HttpDelete("{id}/teachers/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove a teacher from the school")]
        public async Task<IActionResult> RemoveTeacher(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveSchoolTeacherCommand { SchoolId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
