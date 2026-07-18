using ChronicleKeeper.Core.CQRS.PrivilegeLaws.Commands;
using ChronicleKeeper.Core.CQRS.PrivilegeLaws.Queries;
using ChronicleKeeper.Core.DTOs.PrivilegeLaw;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/privilege-laws")]
    [ApiController]
    public class PrivilegeLawController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PrivilegeLawController> _logger;

        public PrivilegeLawController(IMediator mediator, ILogger<PrivilegeLawController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/privilege-laws?worldId=1&socialClassId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get privilege laws", Description = "Returns privilege laws, optionally filtered by world and/or social class")]
        [SwaggerResponse(200, "List of privilege laws", typeof(IEnumerable<PrivilegeLawDto>))]
        public async Task<ActionResult<IEnumerable<PrivilegeLawDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? socialClassId = null)
        {
            var laws = await _mediator.Send(new GetAllPrivilegeLawsQuery { WorldId = worldId, SocialClassId = socialClassId });
            return Ok(laws);
        }

        // GET: /api/privilege-laws/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get privilege law by ID")]
        [SwaggerResponse(200, "Privilege law found", typeof(PrivilegeLawDto))]
        [SwaggerResponse(404, "Privilege law not found")]
        public async Task<ActionResult<PrivilegeLawDto>> GetById(int id)
        {
            var law = await _mediator.Send(new GetPrivilegeLawByIdQuery { Id = id });
            if (law == null) return NotFound();
            return Ok(law);
        }

        // POST: /api/privilege-laws
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new privilege law", Description = "The law's world is derived from its social class")]
        [SwaggerResponse(201, "Privilege law created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<PrivilegeLawDto>> Create([FromBody] PrivilegeLawCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating privilege law: {Name}", dto.Name);
            var result = await _mediator.Send(new CreatePrivilegeLawCommand { PrivilegeLawCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/privilege-laws/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update privilege law by ID", Description = "A law's social class cannot be changed")]
        [SwaggerResponse(200, "Privilege law updated", typeof(PrivilegeLawDto))]
        [SwaggerResponse(404, "Privilege law not found")]
        public async Task<ActionResult<PrivilegeLawDto>> Update(int id, [FromBody] PrivilegeLawUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdatePrivilegeLawCommand { Id = id, PrivilegeLawUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/privilege-laws/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete privilege law by ID")]
        [SwaggerResponse(204, "Privilege law deleted")]
        [SwaggerResponse(404, "Privilege law not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePrivilegeLawCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
