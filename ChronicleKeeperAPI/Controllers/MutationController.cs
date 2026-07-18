using ChronicleKeeper.Core.CQRS.Mutations.Commands;
using ChronicleKeeper.Core.CQRS.Mutations.Queries;
using ChronicleKeeper.Core.DTOs.Mutation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/mutations")]
    [ApiController]
    public class MutationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MutationController> _logger;

        public MutationController(IMediator mediator, ILogger<MutationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/mutations?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get mutations", Description = "Returns mutations, optionally filtered by world")]
        [SwaggerResponse(200, "List of mutations", typeof(IEnumerable<MutationDto>))]
        public async Task<ActionResult<IEnumerable<MutationDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var mutations = await _mediator.Send(new GetAllMutationsQuery { WorldId = worldId });
            return Ok(mutations);
        }

        // GET: /api/mutations/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get mutation by ID", Description = "Returns mutation with its affected creature and history")]
        [SwaggerResponse(200, "Mutation found", typeof(MutationDetailsDto))]
        [SwaggerResponse(404, "Mutation not found")]
        public async Task<ActionResult<MutationDetailsDto>> GetById(int id)
        {
            var mutation = await _mediator.Send(new GetMutationByIdQuery { Id = id });
            if (mutation == null) return NotFound();
            return Ok(mutation);
        }

        // POST: /api/mutations
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new mutation")]
        [SwaggerResponse(201, "Mutation created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<MutationDto>> Create([FromBody] MutationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating mutation: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateMutationCommand { MutationCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/mutations/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update mutation by ID")]
        [SwaggerResponse(200, "Mutation updated", typeof(MutationDto))]
        [SwaggerResponse(404, "Mutation not found")]
        public async Task<ActionResult<MutationDto>> Update(int id, [FromBody] MutationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateMutationCommand { Id = id, MutationUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/mutations/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete mutation by ID")]
        [SwaggerResponse(204, "Mutation deleted")]
        [SwaggerResponse(404, "Mutation not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteMutationCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
