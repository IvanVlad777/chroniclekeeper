using ChronicleKeeper.Core.CQRS.DiplomaticAgreements.Commands;
using ChronicleKeeper.Core.CQRS.DiplomaticAgreements.Queries;
using ChronicleKeeper.Core.DTOs.DiplomaticAgreement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/diplomaticagreements")]
    [ApiController]
    public class DiplomaticAgreementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DiplomaticAgreementController> _logger;

        public DiplomaticAgreementController(IMediator mediator, ILogger<DiplomaticAgreementController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/diplomaticagreements?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get diplomatic agreements", Description = "Returns diplomatic agreements, optionally filtered by world")]
        [SwaggerResponse(200, "List of diplomatic agreements", typeof(IEnumerable<DiplomaticAgreementDto>))]
        public async Task<ActionResult<IEnumerable<DiplomaticAgreementDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var agreements = await _mediator.Send(new GetAllDiplomaticAgreementsQuery { WorldId = worldId });
            return Ok(agreements);
        }

        // GET: /api/diplomaticagreements/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get diplomatic agreement by ID", Description = "Returns diplomatic agreement with both signatory nations")]
        [SwaggerResponse(200, "Diplomatic agreement found", typeof(DiplomaticAgreementDetailsDto))]
        [SwaggerResponse(404, "Diplomatic agreement not found")]
        public async Task<ActionResult<DiplomaticAgreementDetailsDto>> GetById(int id)
        {
            var agreement = await _mediator.Send(new GetDiplomaticAgreementByIdQuery { Id = id });
            if (agreement == null) return NotFound();
            return Ok(agreement);
        }

        // POST: /api/diplomaticagreements
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new diplomatic agreement")]
        [SwaggerResponse(201, "Diplomatic agreement created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<DiplomaticAgreementDto>> Create([FromBody] DiplomaticAgreementCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating diplomatic agreement: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateDiplomaticAgreementCommand { DiplomaticAgreementCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/diplomaticagreements/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update diplomatic agreement by ID")]
        [SwaggerResponse(200, "Diplomatic agreement updated", typeof(DiplomaticAgreementDto))]
        [SwaggerResponse(404, "Diplomatic agreement not found")]
        public async Task<ActionResult<DiplomaticAgreementDto>> Update(int id, [FromBody] DiplomaticAgreementUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateDiplomaticAgreementCommand { Id = id, DiplomaticAgreementUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/diplomaticagreements/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete diplomatic agreement by ID")]
        [SwaggerResponse(204, "Diplomatic agreement deleted")]
        [SwaggerResponse(404, "Diplomatic agreement not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDiplomaticAgreementCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
