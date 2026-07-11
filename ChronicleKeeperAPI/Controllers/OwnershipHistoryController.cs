using ChronicleKeeper.Core.CQRS.OwnershipHistories.Commands;
using ChronicleKeeper.Core.CQRS.OwnershipHistories.Queries;
using ChronicleKeeper.Core.DTOs.OwnershipHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/ownership-history")]
    [ApiController]
    public class OwnershipHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OwnershipHistoryController> _logger;

        public OwnershipHistoryController(IMediator mediator, ILogger<OwnershipHistoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/ownership-history?worldId=1&itemId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get ownership history records", Description = "Returns ownership history records, optionally filtered by world and/or item")]
        [SwaggerResponse(200, "List of ownership history records", typeof(IEnumerable<OwnershipHistoryDto>))]
        public async Task<ActionResult<IEnumerable<OwnershipHistoryDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? itemId = null)
        {
            var histories = await _mediator.Send(new GetAllOwnershipHistoriesQuery { WorldId = worldId, ItemId = itemId });
            return Ok(histories);
        }

        // GET: /api/ownership-history/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get ownership history record by ID")]
        [SwaggerResponse(200, "Ownership history record found", typeof(OwnershipHistoryDto))]
        [SwaggerResponse(404, "Ownership history record not found")]
        public async Task<ActionResult<OwnershipHistoryDto>> GetById(int id)
        {
            var history = await _mediator.Send(new GetOwnershipHistoryByIdQuery { Id = id });
            if (history == null) return NotFound();
            return Ok(history);
        }

        // POST: /api/ownership-history
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new ownership history record", Description = "Record's world is derived from its item")]
        [SwaggerResponse(201, "Ownership history record created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<OwnershipHistoryDto>> Create([FromBody] OwnershipHistoryCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating ownership history: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateOwnershipHistoryCommand { OwnershipHistoryCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/ownership-history/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update ownership history record by ID", Description = "A record's item cannot be changed")]
        [SwaggerResponse(200, "Ownership history record updated", typeof(OwnershipHistoryDto))]
        [SwaggerResponse(404, "Ownership history record not found")]
        public async Task<ActionResult<OwnershipHistoryDto>> Update(int id, [FromBody] OwnershipHistoryUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateOwnershipHistoryCommand { Id = id, OwnershipHistoryUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/ownership-history/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete ownership history record by ID")]
        [SwaggerResponse(204, "Ownership history record deleted")]
        [SwaggerResponse(404, "Ownership history record not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteOwnershipHistoryCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
