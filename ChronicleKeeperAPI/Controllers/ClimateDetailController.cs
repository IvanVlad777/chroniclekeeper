using ChronicleKeeper.Core.CQRS.ClimateDetails.Commands;
using ChronicleKeeper.Core.CQRS.ClimateDetails.Queries;
using ChronicleKeeper.Core.DTOs.ClimateDetail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/climate-details")]
    [ApiController]
    public class ClimateDetailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClimateDetailController> _logger;

        public ClimateDetailController(IMediator mediator, ILogger<ClimateDetailController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/climate-details?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get climate details", Description = "Returns reusable climate details, optionally filtered by world")]
        [SwaggerResponse(200, "List of climate details", typeof(IEnumerable<ClimateDetailDto>))]
        public async Task<ActionResult<IEnumerable<ClimateDetailDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var climateDetails = await _mediator.Send(new GetAllClimateDetailsQuery { WorldId = worldId });
            return Ok(climateDetails);
        }

        // GET: /api/climate-details/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get climate detail by ID")]
        [SwaggerResponse(200, "Climate detail found", typeof(ClimateDetailDto))]
        [SwaggerResponse(404, "Climate detail not found")]
        public async Task<ActionResult<ClimateDetailDto>> GetById(int id)
        {
            var climateDetail = await _mediator.Send(new GetClimateDetailByIdQuery { Id = id });
            if (climateDetail == null) return NotFound();
            return Ok(climateDetail);
        }

        // POST: /api/climate-details
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new climate detail")]
        [SwaggerResponse(201, "Climate detail created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ClimateDetailDto>> Create([FromBody] ClimateDetailCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating climate detail: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateClimateDetailCommand { ClimateDetailCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/climate-details/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update climate detail by ID")]
        [SwaggerResponse(200, "Climate detail updated", typeof(ClimateDetailDto))]
        [SwaggerResponse(404, "Climate detail not found")]
        public async Task<ActionResult<ClimateDetailDto>> Update(int id, [FromBody] ClimateDetailUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateClimateDetailCommand { Id = id, ClimateDetailUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/climate-details/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete climate detail by ID")]
        [SwaggerResponse(204, "Climate detail deleted")]
        [SwaggerResponse(404, "Climate detail not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteClimateDetailCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
