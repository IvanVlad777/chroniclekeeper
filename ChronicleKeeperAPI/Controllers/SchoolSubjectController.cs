using ChronicleKeeper.Core.CQRS.SchoolSubjects.Commands;
using ChronicleKeeper.Core.CQRS.SchoolSubjects.Queries;
using ChronicleKeeper.Core.DTOs.SchoolSubject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/school-subjects")]
    [ApiController]
    public class SchoolSubjectController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SchoolSubjectController> _logger;

        public SchoolSubjectController(IMediator mediator, ILogger<SchoolSubjectController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/school-subjects?worldId=1&schoolId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get school subjects", Description = "Returns school subjects, optionally filtered by world and/or school")]
        [SwaggerResponse(200, "List of school subjects", typeof(IEnumerable<SchoolSubjectDto>))]
        public async Task<ActionResult<IEnumerable<SchoolSubjectDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? schoolId = null)
        {
            var subjects = await _mediator.Send(new GetAllSchoolSubjectsQuery { WorldId = worldId, SchoolId = schoolId });
            return Ok(subjects);
        }

        // GET: /api/school-subjects/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get school subject by ID")]
        [SwaggerResponse(200, "School subject found", typeof(SchoolSubjectDetailsDto))]
        [SwaggerResponse(404, "School subject not found")]
        public async Task<ActionResult<SchoolSubjectDetailsDto>> GetById(int id)
        {
            var subject = await _mediator.Send(new GetSchoolSubjectByIdQuery { Id = id });
            if (subject == null) return NotFound();
            return Ok(subject);
        }

        // POST: /api/school-subjects
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new school subject", Description = "Subject's world is derived from its school")]
        [SwaggerResponse(201, "School subject created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<SchoolSubjectDto>> Create([FromBody] SchoolSubjectCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating school subject: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateSchoolSubjectCommand { SchoolSubjectCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/school-subjects/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update school subject by ID", Description = "A subject's school cannot be changed")]
        [SwaggerResponse(200, "School subject updated", typeof(SchoolSubjectDto))]
        [SwaggerResponse(404, "School subject not found")]
        public async Task<ActionResult<SchoolSubjectDto>> Update(int id, [FromBody] SchoolSubjectUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSchoolSubjectCommand { Id = id, SchoolSubjectUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/school-subjects/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete school subject by ID")]
        [SwaggerResponse(204, "School subject deleted")]
        [SwaggerResponse(404, "School subject not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSchoolSubjectCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/school-subjects/{id}/teachers/{characterId}
        [HttpPost("{id}/teachers/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a teacher (character) to this subject")]
        [SwaggerResponse(204, "Teacher linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddTeacher(int id, int characterId)
        {
            await _mediator.Send(new AddSchoolSubjectTeacherCommand { SchoolSubjectId = id, CharacterId = characterId });
            return NoContent();
        }

        // DELETE: /api/school-subjects/{id}/teachers/{characterId}
        [HttpDelete("{id}/teachers/{characterId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a teacher from this subject")]
        [SwaggerResponse(204, "Teacher unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveTeacher(int id, int characterId)
        {
            var result = await _mediator.Send(new RemoveSchoolSubjectTeacherCommand { SchoolSubjectId = id, CharacterId = characterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
