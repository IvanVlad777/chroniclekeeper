using ChronicleKeeper.Core.CQRS.WeatherPatterns.Commands;
using ChronicleKeeper.Core.CQRS.WeatherPatterns.Queries;
using ChronicleKeeper.Core.DTOs.WeatherPattern;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/weather-patterns")]
    [ApiController]
    public class WeatherPatternController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherPatternController> _logger;

        public WeatherPatternController(IMediator mediator, ILogger<WeatherPatternController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/weather-patterns?worldId=1&climateZoneId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get weather patterns", Description = "Returns weather patterns, optionally filtered by world and/or climate zone")]
        [SwaggerResponse(200, "List of weather patterns", typeof(IEnumerable<WeatherPatternDto>))]
        public async Task<ActionResult<IEnumerable<WeatherPatternDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? climateZoneId = null)
        {
            var weatherPatterns = await _mediator.Send(new GetAllWeatherPatternsQuery { WorldId = worldId, ClimateZoneId = climateZoneId });
            return Ok(weatherPatterns);
        }

        // GET: /api/weather-patterns/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get weather pattern by ID")]
        [SwaggerResponse(200, "Weather pattern found", typeof(WeatherPatternDto))]
        [SwaggerResponse(404, "Weather pattern not found")]
        public async Task<ActionResult<WeatherPatternDto>> GetById(int id)
        {
            var weatherPattern = await _mediator.Send(new GetWeatherPatternByIdQuery { Id = id });
            if (weatherPattern == null) return NotFound();
            return Ok(weatherPattern);
        }

        // POST: /api/weather-patterns
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new weather pattern", Description = "Weather pattern's world is derived from its climate zone")]
        [SwaggerResponse(201, "Weather pattern created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<WeatherPatternDto>> Create([FromBody] WeatherPatternCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating weather pattern: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateWeatherPatternCommand { WeatherPatternCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/weather-patterns/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update weather pattern by ID", Description = "A weather pattern's climate zone cannot be changed")]
        [SwaggerResponse(200, "Weather pattern updated", typeof(WeatherPatternDto))]
        [SwaggerResponse(404, "Weather pattern not found")]
        public async Task<ActionResult<WeatherPatternDto>> Update(int id, [FromBody] WeatherPatternUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateWeatherPatternCommand { Id = id, WeatherPatternUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/weather-patterns/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete weather pattern by ID")]
        [SwaggerResponse(204, "Weather pattern deleted")]
        [SwaggerResponse(404, "Weather pattern not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteWeatherPatternCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
