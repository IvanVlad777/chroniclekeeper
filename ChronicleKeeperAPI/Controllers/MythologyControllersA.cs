using ChronicleKeeper.Core.CQRS.Mythology;
using ChronicleKeeper.Core.DTOs.Mythology;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronicleKeeperAPI.Controllers
{
    [ApiController]
    [Route("api/holy-sites")]
    public class HolySiteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HolySiteController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<HolySiteDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllHolySitesQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<HolySiteDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetHolySiteByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<HolySiteDto>> Create([FromBody] HolySiteCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateHolySiteCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<HolySiteDto>> Update(int id, [FromBody] HolySiteUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateHolySiteCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteHolySiteCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/religious-texts")]
    public class ReligiousTextController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReligiousTextController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ReligiousTextDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllReligiousTextsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ReligiousTextDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetReligiousTextByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ReligiousTextDto>> Create([FromBody] ReligiousTextCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateReligiousTextCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ReligiousTextDto>> Update(int id, [FromBody] ReligiousTextUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateReligiousTextCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteReligiousTextCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/religious-festivals")]
    public class ReligiousFestivalController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReligiousFestivalController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ReligiousFestivalDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllReligiousFestivalsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ReligiousFestivalDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetReligiousFestivalByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ReligiousFestivalDto>> Create([FromBody] ReligiousFestivalCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateReligiousFestivalCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ReligiousFestivalDto>> Update(int id, [FromBody] ReligiousFestivalUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateReligiousFestivalCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteReligiousFestivalCommand { Id = id }) ? NoContent() : NotFound();
    }
}
