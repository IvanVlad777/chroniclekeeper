using ChronicleKeeper.Core.CQRS.Mythology;
using ChronicleKeeper.Core.DTOs.Mythology;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronicleKeeperAPI.Controllers
{
    [ApiController]
    [Route("api/religious-orders")]
    public class ReligiousOrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReligiousOrderController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ReligiousOrderDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllReligiousOrdersQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ReligiousOrderDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetReligiousOrderByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ReligiousOrderDto>> Create([FromBody] ReligiousOrderCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateReligiousOrderCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ReligiousOrderDto>> Update(int id, [FromBody] ReligiousOrderUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateReligiousOrderCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteReligiousOrderCommand { Id = id }) ? NoContent() : NotFound();

        [HttpPost("{id}/factions/{factionId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddFaction(int id, int factionId)
        { await _mediator.Send(new AddReligiousOrderFactionCommand { OrderId = id, FactionId = factionId }); return NoContent(); }

        [HttpDelete("{id}/factions/{factionId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveFaction(int id, int factionId)
        { await _mediator.Send(new RemoveReligiousOrderFactionCommand { OrderId = id, FactionId = factionId }); return NoContent(); }
    }

    [ApiController]
    [Route("api/deities")]
    public class DeityController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeityController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<DeityDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllDeitiesQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<DeityDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetDeityByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<DeityDto>> Create([FromBody] DeityCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateDeityCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<DeityDto>> Update(int id, [FromBody] DeityUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateDeityCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteDeityCommand { Id = id }) ? NoContent() : NotFound();

        [HttpPost("{id}/orders/{orderId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddOrder(int id, int orderId)
        { await _mediator.Send(new AddDeityOrderCommand { DeityId = id, OrderId = orderId }); return NoContent(); }

        [HttpDelete("{id}/orders/{orderId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveOrder(int id, int orderId)
        { await _mediator.Send(new RemoveDeityOrderCommand { DeityId = id, OrderId = orderId }); return NoContent(); }

        [HttpPost("{id}/allies/{alliedDeityId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddAlly(int id, int alliedDeityId)
        { await _mediator.Send(new AddDeityAllyCommand { DeityId = id, AlliedDeityId = alliedDeityId }); return NoContent(); }

        [HttpDelete("{id}/allies/{alliedDeityId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveAlly(int id, int alliedDeityId)
        { await _mediator.Send(new RemoveDeityAllyCommand { DeityId = id, AlliedDeityId = alliedDeityId }); return NoContent(); }

        [HttpPost("{id}/rivals/{rivalDeityId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddRival(int id, int rivalDeityId)
        { await _mediator.Send(new AddDeityRivalCommand { DeityId = id, RivalDeityId = rivalDeityId }); return NoContent(); }

        [HttpDelete("{id}/rivals/{rivalDeityId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveRival(int id, int rivalDeityId)
        { await _mediator.Send(new RemoveDeityRivalCommand { DeityId = id, RivalDeityId = rivalDeityId }); return NoContent(); }
    }
}
