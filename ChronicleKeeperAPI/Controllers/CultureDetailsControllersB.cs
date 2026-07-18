using ChronicleKeeper.Core.CQRS.CultureDetails;
using ChronicleKeeper.Core.DTOs.CultureDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronicleKeeperAPI.Controllers
{
    [ApiController]
    [Route("api/architecture-styles")]
    public class ArchitectureStyleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ArchitectureStyleController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ArchitectureStyleDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllArchitectureStylesQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ArchitectureStyleDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetArchitectureStyleByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ArchitectureStyleDto>> Create([FromBody] ArchitectureStyleCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateArchitectureStyleCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ArchitectureStyleDto>> Update(int id, [FromBody] ArchitectureStyleUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateArchitectureStyleCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteArchitectureStyleCommand { Id = id }) ? NoContent() : NotFound();

        [HttpPost("{id}/locations/{locationId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddLocation(int id, int locationId)
        { await _mediator.Send(new AddArchitectureStyleLocationCommand { ArchitectureStyleId = id, LocationId = locationId }); return NoContent(); }

        [HttpDelete("{id}/locations/{locationId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveLocation(int id, int locationId)
        { await _mediator.Send(new RemoveArchitectureStyleLocationCommand { ArchitectureStyleId = id, LocationId = locationId }); return NoContent(); }
    }

    [ApiController]
    [Route("api/folklore")]
    public class FolkloreController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FolkloreController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<FolkloreDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllFolkloreQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<FolkloreDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetFolkloreByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<FolkloreDto>> Create([FromBody] FolkloreCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateFolkloreCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<FolkloreDto>> Update(int id, [FromBody] FolkloreUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateFolkloreCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteFolkloreCommand { Id = id }) ? NoContent() : NotFound();

        [HttpPost("{id}/events/{eventId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddEvent(int id, int eventId)
        { await _mediator.Send(new AddFolkloreEventCommand { FolkloreId = id, EventId = eventId }); return NoContent(); }

        [HttpDelete("{id}/events/{eventId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveEvent(int id, int eventId)
        { await _mediator.Send(new RemoveFolkloreEventCommand { FolkloreId = id, EventId = eventId }); return NoContent(); }

        [HttpPost("{id}/species/{speciesId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddSpecies(int id, int speciesId)
        { await _mediator.Send(new AddFolkloreSpeciesCommand { FolkloreId = id, SpeciesId = speciesId }); return NoContent(); }

        [HttpDelete("{id}/species/{speciesId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveSpecies(int id, int speciesId)
        { await _mediator.Send(new RemoveFolkloreSpeciesCommand { FolkloreId = id, SpeciesId = speciesId }); return NoContent(); }
    }

    [ApiController]
    [Route("api/myths")]
    public class MythController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MythController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<MythDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllMythsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<MythDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetMythByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MythDto>> Create([FromBody] MythCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateMythCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MythDto>> Update(int id, [FromBody] MythUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateMythCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteMythCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/cultural-festivals")]
    public class CulturalFestivalController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CulturalFestivalController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<CulturalFestivalDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllCulturalFestivalsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<CulturalFestivalDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetCulturalFestivalByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CulturalFestivalDto>> Create([FromBody] CulturalFestivalCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateCulturalFestivalCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CulturalFestivalDto>> Update(int id, [FromBody] CulturalFestivalUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateCulturalFestivalCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteCulturalFestivalCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/cultural-institutions")]
    public class CulturalInstitutionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CulturalInstitutionController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<CulturalInstitutionDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllCulturalInstitutionsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<CulturalInstitutionDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetCulturalInstitutionByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CulturalInstitutionDto>> Create([FromBody] CulturalInstitutionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateCulturalInstitutionCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CulturalInstitutionDto>> Update(int id, [FromBody] CulturalInstitutionUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateCulturalInstitutionCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteCulturalInstitutionCommand { Id = id }) ? NoContent() : NotFound();
    }
}
