using ChronicleKeeper.Core.CQRS.Items.Commands;
using ChronicleKeeper.Core.CQRS.Items.Queries;
using ChronicleKeeper.Core.DTOs.Item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IMediator mediator, ILogger<ItemController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/items?worldId=1&currentOwnerId=2&factionId=3
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get items", Description = "Returns items, optionally filtered by world, current owner and/or faction")]
        [SwaggerResponse(200, "List of items", typeof(IEnumerable<ItemDto>))]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? currentOwnerId = null, [FromQuery] int? factionId = null)
        {
            var items = await _mediator.Send(new GetAllItemsQuery { WorldId = worldId, CurrentOwnerId = currentOwnerId, FactionId = factionId });
            return Ok(items);
        }

        // GET: /api/items/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get item by ID")]
        [SwaggerResponse(200, "Item found", typeof(ItemDetailsDto))]
        [SwaggerResponse(404, "Item not found")]
        public async Task<ActionResult<ItemDetailsDto>> GetById(int id)
        {
            var item = await _mediator.Send(new GetItemByIdQuery { Id = id });
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: /api/items
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new item")]
        [SwaggerResponse(201, "Item created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ItemDto>> Create([FromBody] ItemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating item: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateItemCommand { ItemCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/items/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update item by ID")]
        [SwaggerResponse(200, "Item updated", typeof(ItemDto))]
        [SwaggerResponse(404, "Item not found")]
        public async Task<ActionResult<ItemDto>> Update(int id, [FromBody] ItemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateItemCommand { Id = id, ItemUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/items/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete item by ID")]
        [SwaggerResponse(204, "Item deleted")]
        [SwaggerResponse(404, "Item not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteItemCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
