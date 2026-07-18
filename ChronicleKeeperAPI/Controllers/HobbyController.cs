using ChronicleKeeper.Core.CQRS.Hobbies.Commands;
using ChronicleKeeper.Core.CQRS.Hobbies.Queries;
using ChronicleKeeper.Core.DTOs.Hobby;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/hobbies")]
    [ApiController]
    public class HobbyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HobbyController> _logger;

        public HobbyController(IMediator mediator, ILogger<HobbyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/hobbies?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get hobbies", Description = "Returns hobbies, optionally filtered by world")]
        [SwaggerResponse(200, "List of hobbies", typeof(IEnumerable<HobbyDto>))]
        public async Task<ActionResult<IEnumerable<HobbyDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var hobbies = await _mediator.Send(new GetAllHobbiesQuery { WorldId = worldId });
            return Ok(hobbies);
        }

        // GET: /api/hobbies/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get hobby by ID")]
        [SwaggerResponse(200, "Hobby found", typeof(HobbyDetailsDto))]
        [SwaggerResponse(404, "Hobby not found")]
        public async Task<ActionResult<HobbyDetailsDto>> GetById(int id)
        {
            var hobby = await _mediator.Send(new GetHobbyByIdQuery { Id = id });
            if (hobby == null) return NotFound();
            return Ok(hobby);
        }

        // POST: /api/hobbies
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new hobby")]
        [SwaggerResponse(201, "Hobby created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<HobbyDto>> Create([FromBody] HobbyCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating hobby: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateHobbyCommand { HobbyCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/hobbies/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update hobby by ID")]
        [SwaggerResponse(200, "Hobby updated", typeof(HobbyDto))]
        [SwaggerResponse(404, "Hobby not found")]
        public async Task<ActionResult<HobbyDto>> Update(int id, [FromBody] HobbyUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateHobbyCommand { Id = id, HobbyUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/hobbies/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete hobby by ID")]
        [SwaggerResponse(204, "Hobby deleted")]
        [SwaggerResponse(404, "Hobby not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteHobbyCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
