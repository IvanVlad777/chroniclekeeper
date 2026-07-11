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
    }
}
