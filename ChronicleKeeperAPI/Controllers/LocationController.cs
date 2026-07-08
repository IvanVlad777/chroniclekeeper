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
    }
}
