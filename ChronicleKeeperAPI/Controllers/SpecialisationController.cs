using ChronicleKeeper.Core.CQRS.Specialisations.Commands;
using ChronicleKeeper.Core.CQRS.Specialisations.Queries;
using ChronicleKeeper.Core.DTOs.Specialisation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/specialisations")]
    [ApiController]
    public class SpecialisationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SpecialisationController> _logger;

        public SpecialisationController(IMediator mediator, ILogger<SpecialisationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/specialisations?worldId=1&professionId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get specialisations", Description = "Returns specialisations, optionally filtered by world and/or profession")]
        [SwaggerResponse(200, "List of specialisations", typeof(IEnumerable<SpecialisationDto>))]
        public async Task<ActionResult<IEnumerable<SpecialisationDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? professionId = null)
        {
            var specialisations = await _mediator.Send(new GetAllSpecialisationsQuery { WorldId = worldId, ProfessionId = professionId });
            return Ok(specialisations);
        }

        // GET: /api/specialisations/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get specialisation by ID")]
        [SwaggerResponse(200, "Specialisation found", typeof(SpecialisationDto))]
        [SwaggerResponse(404, "Specialisation not found")]
        public async Task<ActionResult<SpecialisationDto>> GetById(int id)
        {
            var specialisation = await _mediator.Send(new GetSpecialisationByIdQuery { Id = id });
            if (specialisation == null) return NotFound();
            return Ok(specialisation);
        }

        // POST: /api/specialisations
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new specialisation", Description = "Specialisation's world is derived from its profession")]
        [SwaggerResponse(201, "Specialisation created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<SpecialisationDto>> Create([FromBody] SpecialisationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating specialisation: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateSpecialisationCommand { SpecialisationCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/specialisations/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update specialisation by ID", Description = "A specialisation's profession cannot be changed")]
        [SwaggerResponse(200, "Specialisation updated", typeof(SpecialisationDto))]
        [SwaggerResponse(404, "Specialisation not found")]
        public async Task<ActionResult<SpecialisationDto>> Update(int id, [FromBody] SpecialisationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSpecialisationCommand { Id = id, SpecialisationUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/specialisations/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete specialisation by ID")]
        [SwaggerResponse(204, "Specialisation deleted")]
        [SwaggerResponse(404, "Specialisation not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSpecialisationCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
