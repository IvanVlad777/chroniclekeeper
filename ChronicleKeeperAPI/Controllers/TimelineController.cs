using ChronicleKeeper.Core.CQRS.Timelines.Commands;
using ChronicleKeeper.Core.CQRS.Timelines.Queries;
using ChronicleKeeper.Core.DTOs.Timeline;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/timelines")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TimelineController> _logger;

        public TimelineController(IMediator mediator, ILogger<TimelineController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/timelines?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get timelines", Description = "Returns timelines, optionally filtered by world")]
        [SwaggerResponse(200, "List of timelines", typeof(IEnumerable<TimelineDto>))]
        public async Task<ActionResult<IEnumerable<TimelineDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var timelines = await _mediator.Send(new GetAllTimelinesQuery { WorldId = worldId });
            return Ok(timelines);
        }

        // GET: /api/timelines/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get timeline by ID", Description = "Returns the timeline with its events ordered by SortOrder")]
        [SwaggerResponse(200, "Timeline found", typeof(TimelineDetailsDto))]
        [SwaggerResponse(404, "Timeline not found")]
        public async Task<ActionResult<TimelineDetailsDto>> GetById(int id)
        {
            var timeline = await _mediator.Send(new GetTimelineByIdQuery { Id = id });
            if (timeline == null) return NotFound();
            return Ok(timeline);
        }

        // POST: /api/timelines
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new timeline")]
        [SwaggerResponse(201, "Timeline created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<TimelineDto>> Create([FromBody] TimelineCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating timeline: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateTimelineCommand { TimelineCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/timelines/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update timeline by ID")]
        [SwaggerResponse(200, "Timeline updated", typeof(TimelineDto))]
        [SwaggerResponse(404, "Timeline not found")]
        public async Task<ActionResult<TimelineDto>> Update(int id, [FromBody] TimelineUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateTimelineCommand { Id = id, TimelineUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/timelines/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete timeline by ID", Description = "Its events are removed automatically")]
        [SwaggerResponse(204, "Timeline deleted")]
        [SwaggerResponse(404, "Timeline not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTimelineCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/timelines/{id}/events
        [HttpPost("{id}/events")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Add event to timeline", Description = "Event inherits the timeline's world")]
        [SwaggerResponse(201, "Event created", typeof(TimelineEventDto))]
        [SwaggerResponse(404, "Timeline not found")]
        public async Task<ActionResult<TimelineEventDto>> CreateEvent(int id, [FromBody] TimelineEventCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateTimelineEventCommand { TimelineId = id, EventCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, result);
        }

        // PUT: /api/timelines/events/{eventId}
        [HttpPut("events/{eventId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update timeline event")]
        [SwaggerResponse(200, "Event updated", typeof(TimelineEventDto))]
        [SwaggerResponse(404, "Event not found")]
        public async Task<ActionResult<TimelineEventDto>> UpdateEvent(int eventId, [FromBody] TimelineEventUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateTimelineEventCommand { EventId = eventId, EventUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/timelines/events/{eventId}
        [HttpDelete("events/{eventId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete timeline event")]
        [SwaggerResponse(204, "Event deleted")]
        [SwaggerResponse(404, "Event not found")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var result = await _mediator.Send(new DeleteTimelineEventCommand { EventId = eventId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
