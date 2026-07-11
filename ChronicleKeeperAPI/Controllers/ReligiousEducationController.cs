using ChronicleKeeper.Core.CQRS.ReligiousEducations.Commands;
using ChronicleKeeper.Core.CQRS.ReligiousEducations.Queries;
using ChronicleKeeper.Core.DTOs.ReligiousEducation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/religious-educations")]
    [ApiController]
    public class ReligiousEducationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReligiousEducationController> _logger;

        public ReligiousEducationController(IMediator mediator, ILogger<ReligiousEducationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/religious-educations?worldId=1&characterId=2&religionId=3
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get religious educations", Description = "Returns religious educations, optionally filtered by world, character and/or religion")]
        [SwaggerResponse(200, "List of religious educations", typeof(IEnumerable<ReligiousEducationDto>))]
        public async Task<ActionResult<IEnumerable<ReligiousEducationDto>>> GetAll(
            [FromQuery] int? worldId = null,
            [FromQuery] int? characterId = null,
            [FromQuery] int? religionId = null)
        {
            var records = await _mediator.Send(new GetAllReligiousEducationsQuery
            {
                WorldId = worldId,
                CharacterId = characterId,
                ReligionId = religionId
            });
            return Ok(records);
        }

        // GET: /api/religious-educations/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get religious education by ID")]
        [SwaggerResponse(200, "Religious education found", typeof(ReligiousEducationDto))]
        [SwaggerResponse(404, "Religious education not found")]
        public async Task<ActionResult<ReligiousEducationDto>> GetById(int id)
        {
            var record = await _mediator.Send(new GetReligiousEducationByIdQuery { Id = id });
            if (record == null) return NotFound();
            return Ok(record);
        }

        // POST: /api/religious-educations
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new religious education", Description = "Religious education's world is derived from its religion")]
        [SwaggerResponse(201, "Religious education created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ReligiousEducationDto>> Create([FromBody] ReligiousEducationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating religious education: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateReligiousEducationCommand { ReligiousEducationCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/religious-educations/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update religious education by ID", Description = "A religious education's religion cannot be changed")]
        [SwaggerResponse(200, "Religious education updated", typeof(ReligiousEducationDto))]
        [SwaggerResponse(404, "Religious education not found")]
        public async Task<ActionResult<ReligiousEducationDto>> Update(int id, [FromBody] ReligiousEducationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateReligiousEducationCommand { Id = id, ReligiousEducationUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/religious-educations/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete religious education by ID")]
        [SwaggerResponse(204, "Religious education deleted")]
        [SwaggerResponse(404, "Religious education not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteReligiousEducationCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
