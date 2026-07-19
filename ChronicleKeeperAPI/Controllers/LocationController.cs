using ChronicleKeeper.Core.CQRS.Locations.Commands;
using ChronicleKeeper.Core.CQRS.Locations.Queries;
using ChronicleKeeper.Core.DTOs.Location;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IMediator mediator, ILogger<LocationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/locations?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get locations", Description = "Returns locations, optionally filtered by world")]
        [SwaggerResponse(200, "List of locations", typeof(IEnumerable<LocationDto>))]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var locations = await _mediator.Send(new GetAllLocationsQuery { WorldId = worldId });
            return Ok(locations);
        }

        // GET: /api/locations/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get location by ID", Description = "Returns detailed location info (parent, sub-locations, tags)")]
        [SwaggerResponse(200, "Location found", typeof(LocationDetailsDto))]
        [SwaggerResponse(404, "Location not found")]
        public async Task<ActionResult<LocationDetailsDto>> GetById(int id)
        {
            var location = await _mediator.Send(new GetLocationByIdQuery { Id = id });
            if (location == null) return NotFound();
            return Ok(location);
        }

        // POST: /api/locations
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new location")]
        [SwaggerResponse(201, "Location created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<LocationDto>> Create([FromBody] LocationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating location: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateLocationCommand { LocationCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/locations/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update location by ID")]
        [SwaggerResponse(200, "Location updated", typeof(LocationDto))]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<ActionResult<LocationDto>> Update(int id, [FromBody] LocationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateLocationCommand { Id = id, LocationUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/locations/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete location by ID", Description = "Fails with 400 if the location has sub-locations")]
        [SwaggerResponse(204, "Location deleted")]
        [SwaggerResponse(400, "Location has sub-locations")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteLocationCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/locations/{id}/native-species/{speciesId}
        [HttpPost("{id}/native-species/{speciesId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a native species to the region")]
        [SwaggerResponse(204, "Species linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddNativeSpecies(int id, int speciesId)
        {
            await _mediator.Send(new AddRegionNativeSpeciesCommand { RegionId = id, SapientSpeciesId = speciesId });
            return NoContent();
        }

        // DELETE: /api/locations/{id}/native-species/{speciesId}
        [HttpDelete("{id}/native-species/{speciesId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a native species from the region")]
        [SwaggerResponse(204, "Species unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveNativeSpecies(int id, int speciesId)
        {
            var result = await _mediator.Send(new RemoveRegionNativeSpeciesCommand { RegionId = id, SapientSpeciesId = speciesId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/locations/{id}/links/{targetType}/{targetId}
        [HttpPost("{id}/links/{targetType}/{targetId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Cross-link a Country/City to an entity",
            Description = "TargetType: Industry | Corporation | Guild | PoliticalParty | Nation | Faction (Country only) | Culture | Religion | CulturalInstitution (City only)")]
        [SwaggerResponse(204, "Linked")]
        [SwaggerResponse(400, "Invalid target / already linked / unsupported for this location type")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<IActionResult> AddCrossLink(int id, LocationLinkTargetType targetType, int targetId)
        {
            await _mediator.Send(new AddLocationCrossLinkCommand { LocationId = id, TargetType = targetType, TargetId = targetId });
            return NoContent();
        }

        // DELETE: /api/locations/{id}/links/{targetType}/{targetId}
        [HttpDelete("{id}/links/{targetType}/{targetId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove a Country/City cross-link")]
        [SwaggerResponse(204, "Unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveCrossLink(int id, LocationLinkTargetType targetType, int targetId)
        {
            var result = await _mediator.Send(new RemoveLocationCrossLinkCommand { LocationId = id, TargetType = targetType, TargetId = targetId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
