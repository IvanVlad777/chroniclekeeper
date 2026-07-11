using ChronicleKeeper.Core.CQRS.ClimateZones.Commands;
using ChronicleKeeper.Core.CQRS.ClimateZones.Queries;
using ChronicleKeeper.Core.DTOs.ClimateZone;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/climate-zones")]
    [ApiController]
    public class ClimateZoneController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClimateZoneController> _logger;

        public ClimateZoneController(IMediator mediator, ILogger<ClimateZoneController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/climate-zones?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get climate zones", Description = "Returns climate zones, optionally filtered by world")]
        [SwaggerResponse(200, "List of climate zones", typeof(IEnumerable<ClimateZoneDto>))]
        public async Task<ActionResult<IEnumerable<ClimateZoneDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var climateZones = await _mediator.Send(new GetAllClimateZonesQuery { WorldId = worldId });
            return Ok(climateZones);
        }

        // GET: /api/climate-zones/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get climate zone by ID", Description = "Returns climate zone with its details, seasons, locations and weather patterns")]
        [SwaggerResponse(200, "Climate zone found", typeof(ClimateZoneDetailsDto))]
        [SwaggerResponse(404, "Climate zone not found")]
        public async Task<ActionResult<ClimateZoneDetailsDto>> GetById(int id)
        {
            var climateZone = await _mediator.Send(new GetClimateZoneByIdQuery { Id = id });
            if (climateZone == null) return NotFound();
            return Ok(climateZone);
        }

        // POST: /api/climate-zones
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new climate zone")]
        [SwaggerResponse(201, "Climate zone created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ClimateZoneDto>> Create([FromBody] ClimateZoneCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating climate zone: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateClimateZoneCommand { ClimateZoneCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/climate-zones/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update climate zone by ID")]
        [SwaggerResponse(200, "Climate zone updated", typeof(ClimateZoneDto))]
        [SwaggerResponse(404, "Climate zone not found")]
        public async Task<ActionResult<ClimateZoneDto>> Update(int id, [FromBody] ClimateZoneUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateClimateZoneCommand { Id = id, ClimateZoneUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/climate-zones/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete climate zone by ID")]
        [SwaggerResponse(204, "Climate zone deleted")]
        [SwaggerResponse(404, "Climate zone not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteClimateZoneCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/climate-zones/{id}/details/{detailId}
        [HttpPost("{id}/details/{detailId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a climate detail to the zone")]
        [SwaggerResponse(204, "Climate detail linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddDetail(int id, int detailId)
        {
            await _mediator.Send(new AddClimateZoneDetailCommand { ClimateZoneId = id, ClimateDetailId = detailId });
            return NoContent();
        }

        // DELETE: /api/climate-zones/{id}/details/{detailId}
        [HttpDelete("{id}/details/{detailId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a climate detail from the zone")]
        [SwaggerResponse(204, "Climate detail unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveDetail(int id, int detailId)
        {
            var result = await _mediator.Send(new RemoveClimateZoneDetailCommand { ClimateZoneId = id, ClimateDetailId = detailId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/climate-zones/{id}/seasons/{seasonId}
        [HttpPost("{id}/seasons/{seasonId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a season to the zone")]
        [SwaggerResponse(204, "Season linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddSeason(int id, int seasonId)
        {
            await _mediator.Send(new AddClimateZoneSeasonCommand { ClimateZoneId = id, SeasonId = seasonId });
            return NoContent();
        }

        // DELETE: /api/climate-zones/{id}/seasons/{seasonId}
        [HttpDelete("{id}/seasons/{seasonId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a season from the zone")]
        [SwaggerResponse(204, "Season unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveSeason(int id, int seasonId)
        {
            var result = await _mediator.Send(new RemoveClimateZoneSeasonCommand { ClimateZoneId = id, SeasonId = seasonId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/climate-zones/{id}/locations/{locationId}
        [HttpPost("{id}/locations/{locationId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a location to the zone")]
        [SwaggerResponse(204, "Location linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddLocation(int id, int locationId)
        {
            await _mediator.Send(new AddClimateZoneLocationCommand { ClimateZoneId = id, LocationId = locationId });
            return NoContent();
        }

        // DELETE: /api/climate-zones/{id}/locations/{locationId}
        [HttpDelete("{id}/locations/{locationId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a location from the zone")]
        [SwaggerResponse(204, "Location unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveLocation(int id, int locationId)
        {
            var result = await _mediator.Send(new RemoveClimateZoneLocationCommand { ClimateZoneId = id, LocationId = locationId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
