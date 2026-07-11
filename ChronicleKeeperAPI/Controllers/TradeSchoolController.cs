using ChronicleKeeper.Core.CQRS.TradeSchools.Commands;
using ChronicleKeeper.Core.CQRS.TradeSchools.Queries;
using ChronicleKeeper.Core.DTOs.TradeSchool;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/trade-schools")]
    [ApiController]
    public class TradeSchoolController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TradeSchoolController> _logger;

        public TradeSchoolController(IMediator mediator, ILogger<TradeSchoolController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/trade-schools?worldId=1&educationSystemId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get trade schools", Description = "Returns trade schools, optionally filtered by world and/or education system")]
        [SwaggerResponse(200, "List of trade schools", typeof(IEnumerable<TradeSchoolDto>))]
        public async Task<ActionResult<IEnumerable<TradeSchoolDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? educationSystemId = null)
        {
            var tradeSchools = await _mediator.Send(new GetAllTradeSchoolsQuery { WorldId = worldId, EducationSystemId = educationSystemId });
            return Ok(tradeSchools);
        }

        // GET: /api/trade-schools/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get trade school by ID", Description = "Returns trade school with its subjects, alumni, trained professions and apprenticeships")]
        [SwaggerResponse(200, "Trade school found", typeof(TradeSchoolDetailsDto))]
        [SwaggerResponse(404, "Trade school not found")]
        public async Task<ActionResult<TradeSchoolDetailsDto>> GetById(int id)
        {
            var tradeSchool = await _mediator.Send(new GetTradeSchoolByIdQuery { Id = id });
            if (tradeSchool == null) return NotFound();
            return Ok(tradeSchool);
        }

        // POST: /api/trade-schools
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new trade school", Description = "Trade school's world is derived from its education system")]
        [SwaggerResponse(201, "Trade school created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<TradeSchoolDto>> Create([FromBody] TradeSchoolCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating trade school: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateTradeSchoolCommand { TradeSchoolCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/trade-schools/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update trade school by ID", Description = "A trade school's education system cannot be changed")]
        [SwaggerResponse(200, "Trade school updated", typeof(TradeSchoolDto))]
        [SwaggerResponse(404, "Trade school not found")]
        public async Task<ActionResult<TradeSchoolDto>> Update(int id, [FromBody] TradeSchoolUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateTradeSchoolCommand { Id = id, TradeSchoolUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/trade-schools/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete trade school by ID", Description = "Fails with 400 if any apprenticeship or education record uses the trade school")]
        [SwaggerResponse(204, "Trade school deleted")]
        [SwaggerResponse(400, "Trade school in use")]
        [SwaggerResponse(404, "Trade school not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTradeSchoolCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
