using ChronicleKeeper.Core.CQRS.EducationRecords.Commands;
using ChronicleKeeper.Core.CQRS.EducationRecords.Queries;
using ChronicleKeeper.Core.DTOs.EducationRecord;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/education-records")]
    [ApiController]
    public class EducationRecordController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EducationRecordController> _logger;

        public EducationRecordController(IMediator mediator, ILogger<EducationRecordController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/education-records?worldId=1&characterId=2&schoolId=3&universityId=4
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get education records", Description = "Returns education records, optionally filtered by world, character, school and/or university")]
        [SwaggerResponse(200, "List of education records", typeof(IEnumerable<EducationRecordDto>))]
        public async Task<ActionResult<IEnumerable<EducationRecordDto>>> GetAll(
            [FromQuery] int? worldId = null,
            [FromQuery] int? characterId = null,
            [FromQuery] int? schoolId = null,
            [FromQuery] int? universityId = null)
        {
            var records = await _mediator.Send(new GetAllEducationRecordsQuery
            {
                WorldId = worldId,
                CharacterId = characterId,
                SchoolId = schoolId,
                UniversityId = universityId
            });
            return Ok(records);
        }

        // GET: /api/education-records/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get education record by ID")]
        [SwaggerResponse(200, "Education record found", typeof(EducationRecordDto))]
        [SwaggerResponse(404, "Education record not found")]
        public async Task<ActionResult<EducationRecordDto>> GetById(int id)
        {
            var record = await _mediator.Send(new GetEducationRecordByIdQuery { Id = id });
            if (record == null) return NotFound();
            return Ok(record);
        }

        // POST: /api/education-records
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new education record")]
        [SwaggerResponse(201, "Education record created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<EducationRecordDto>> Create([FromBody] EducationRecordCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating education record: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateEducationRecordCommand { EducationRecordCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/education-records/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update education record by ID")]
        [SwaggerResponse(200, "Education record updated", typeof(EducationRecordDto))]
        [SwaggerResponse(404, "Education record not found")]
        public async Task<ActionResult<EducationRecordDto>> Update(int id, [FromBody] EducationRecordUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateEducationRecordCommand { Id = id, EducationRecordUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/education-records/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete education record by ID")]
        [SwaggerResponse(204, "Education record deleted")]
        [SwaggerResponse(404, "Education record not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEducationRecordCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
