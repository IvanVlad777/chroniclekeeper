using ChronicleKeeper.Core.CQRS.Histories.Commands;
using ChronicleKeeper.Core.CQRS.Histories.Queries;
using ChronicleKeeper.Core.DTOs.History;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/histories")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(IMediator mediator, ILogger<HistoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/histories?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get histories", Description = "Returns histories, optionally filtered by world")]
        [SwaggerResponse(200, "List of histories", typeof(IEnumerable<HistoryDto>))]
        public async Task<ActionResult<IEnumerable<HistoryDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var histories = await _mediator.Send(new GetAllHistoriesQuery { WorldId = worldId });
            return Ok(histories);
        }

        // GET: /api/histories/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get history by ID")]
        [SwaggerResponse(200, "History found", typeof(HistoryDetailsDto))]
        [SwaggerResponse(404, "History not found")]
        public async Task<ActionResult<HistoryDetailsDto>> GetById(int id)
        {
            var history = await _mediator.Send(new GetHistoryByIdQuery { Id = id });
            if (history == null) return NotFound();
            return Ok(history);
        }

        // POST: /api/histories
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new history")]
        [SwaggerResponse(201, "History created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<HistoryDto>> Create([FromBody] HistoryCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating history: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateHistoryCommand { HistoryCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/histories/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update history by ID")]
        [SwaggerResponse(200, "History updated", typeof(HistoryDto))]
        [SwaggerResponse(404, "History not found")]
        public async Task<ActionResult<HistoryDto>> Update(int id, [FromBody] HistoryUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateHistoryCommand { Id = id, HistoryUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/histories/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete history by ID")]
        [SwaggerResponse(204, "History deleted")]
        [SwaggerResponse(404, "History not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteHistoryCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
