using ChronicleKeeper.Core.CQRS.Apprenticeships.Commands;
using ChronicleKeeper.Core.CQRS.Apprenticeships.Queries;
using ChronicleKeeper.Core.DTOs.Apprenticeship;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/apprenticeships")]
    [ApiController]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeshipController> _logger;

        public ApprenticeshipController(IMediator mediator, ILogger<ApprenticeshipController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/apprenticeships?worldId=1&professionId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get apprenticeships", Description = "Returns apprenticeships, optionally filtered by world and/or profession")]
        [SwaggerResponse(200, "List of apprenticeships", typeof(IEnumerable<ApprenticeshipDto>))]
        public async Task<ActionResult<IEnumerable<ApprenticeshipDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? professionId = null)
        {
            var apprenticeships = await _mediator.Send(new GetAllApprenticeshipsQuery { WorldId = worldId, ProfessionId = professionId });
            return Ok(apprenticeships);
        }

        // GET: /api/apprenticeships/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get apprenticeship by ID")]
        [SwaggerResponse(200, "Apprenticeship found", typeof(ApprenticeshipDto))]
        [SwaggerResponse(404, "Apprenticeship not found")]
        public async Task<ActionResult<ApprenticeshipDto>> GetById(int id)
        {
            var apprenticeship = await _mediator.Send(new GetApprenticeshipByIdQuery { Id = id });
            if (apprenticeship == null) return NotFound();
            return Ok(apprenticeship);
        }

        // POST: /api/apprenticeships
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new apprenticeship", Description = "Apprenticeship's world is derived from its profession")]
        [SwaggerResponse(201, "Apprenticeship created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ApprenticeshipDto>> Create([FromBody] ApprenticeshipCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating apprenticeship: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateApprenticeshipCommand { ApprenticeshipCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/apprenticeships/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update apprenticeship by ID", Description = "An apprenticeship's profession cannot be changed")]
        [SwaggerResponse(200, "Apprenticeship updated", typeof(ApprenticeshipDto))]
        [SwaggerResponse(404, "Apprenticeship not found")]
        public async Task<ActionResult<ApprenticeshipDto>> Update(int id, [FromBody] ApprenticeshipUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateApprenticeshipCommand { Id = id, ApprenticeshipUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/apprenticeships/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete apprenticeship by ID")]
        [SwaggerResponse(204, "Apprenticeship deleted")]
        [SwaggerResponse(404, "Apprenticeship not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteApprenticeshipCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
