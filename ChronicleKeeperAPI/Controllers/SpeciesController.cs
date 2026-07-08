using ChronicleKeeper.Core.CQRS.Species.Commands;
using ChronicleKeeper.Core.CQRS.Species.Queries;
using ChronicleKeeper.Core.DTOs.Species;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/species")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SpeciesController> _logger;

        public SpeciesController(IMediator mediator, ILogger<SpeciesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/species?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get species", Description = "Returns species, optionally filtered by world")]
        [SwaggerResponse(200, "List of species", typeof(IEnumerable<SpeciesDto>))]
        public async Task<ActionResult<IEnumerable<SpeciesDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var species = await _mediator.Send(new GetAllSpeciesQuery { WorldId = worldId });
            return Ok(species);
        }

        // GET: /api/species/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get species by ID", Description = "Returns species with its races")]
        [SwaggerResponse(200, "Species found", typeof(SpeciesDetailsDto))]
        [SwaggerResponse(404, "Species not found")]
        public async Task<ActionResult<SpeciesDetailsDto>> GetById(int id)
        {
            var species = await _mediator.Send(new GetSpeciesByIdQuery { Id = id });
            if (species == null) return NotFound();
            return Ok(species);
        }

        // POST: /api/species
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new species")]
        [SwaggerResponse(201, "Species created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<SpeciesDto>> Create([FromBody] SpeciesCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating species: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateSpeciesCommand { SpeciesCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/species/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update species by ID")]
        [SwaggerResponse(200, "Species updated", typeof(SpeciesDto))]
        [SwaggerResponse(404, "Species not found")]
        public async Task<ActionResult<SpeciesDto>> Update(int id, [FromBody] SpeciesUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateSpeciesCommand { Id = id, SpeciesUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/species/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete species by ID", Description = "Fails with 400 if any character uses the species or one of its races")]
        [SwaggerResponse(204, "Species deleted")]
        [SwaggerResponse(400, "Species in use")]
        [SwaggerResponse(404, "Species not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSpeciesCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/races")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RaceController> _logger;

        public RaceController(IMediator mediator, ILogger<RaceController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/races?worldId=1&speciesId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get races", Description = "Returns races, optionally filtered by world and/or species")]
        [SwaggerResponse(200, "List of races", typeof(IEnumerable<RaceDto>))]
        public async Task<ActionResult<IEnumerable<RaceDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? speciesId = null)
        {
            var races = await _mediator.Send(new GetAllRacesQuery { WorldId = worldId, SpeciesId = speciesId });
            return Ok(races);
        }

        // GET: /api/races/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get race by ID")]
        [SwaggerResponse(200, "Race found", typeof(RaceDto))]
        [SwaggerResponse(404, "Race not found")]
        public async Task<ActionResult<RaceDto>> GetById(int id)
        {
            var race = await _mediator.Send(new GetRaceByIdQuery { Id = id });
            if (race == null) return NotFound();
            return Ok(race);
        }

        // POST: /api/races
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new race", Description = "Race's world is derived from its species")]
        [SwaggerResponse(201, "Race created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<RaceDto>> Create([FromBody] RaceCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating race: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateRaceCommand { RaceCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/races/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update race by ID", Description = "A race's species cannot be changed")]
        [SwaggerResponse(200, "Race updated", typeof(RaceDto))]
        [SwaggerResponse(404, "Race not found")]
        public async Task<ActionResult<RaceDto>> Update(int id, [FromBody] RaceUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateRaceCommand { Id = id, RaceUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/races/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete race by ID", Description = "Fails with 400 if any character uses the race")]
        [SwaggerResponse(204, "Race deleted")]
        [SwaggerResponse(400, "Race in use")]
        [SwaggerResponse(404, "Race not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteRaceCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
