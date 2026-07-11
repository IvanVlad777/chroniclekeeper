using ChronicleKeeper.Core.CQRS.Professions.Commands;
using ChronicleKeeper.Core.CQRS.Professions.Queries;
using ChronicleKeeper.Core.DTOs.Profession;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/professions")]
    [ApiController]
    public class ProfessionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProfessionController> _logger;

        public ProfessionController(IMediator mediator, ILogger<ProfessionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/professions?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get professions", Description = "Returns professions, optionally filtered by world")]
        [SwaggerResponse(200, "List of professions", typeof(IEnumerable<ProfessionDto>))]
        public async Task<ActionResult<IEnumerable<ProfessionDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var professions = await _mediator.Send(new GetAllProfessionsQuery { WorldId = worldId });
            return Ok(professions);
        }

        // GET: /api/professions/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get profession by ID", Description = "Returns profession with its job ranks, apprenticeships, specialisations and cross-links")]
        [SwaggerResponse(200, "Profession found", typeof(ProfessionDetailsDto))]
        [SwaggerResponse(404, "Profession not found")]
        public async Task<ActionResult<ProfessionDetailsDto>> GetById(int id)
        {
            var profession = await _mediator.Send(new GetProfessionByIdQuery { Id = id });
            if (profession == null) return NotFound();
            return Ok(profession);
        }

        // POST: /api/professions
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new profession")]
        [SwaggerResponse(201, "Profession created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ProfessionDto>> Create([FromBody] ProfessionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating profession: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateProfessionCommand { ProfessionCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/professions/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update profession by ID")]
        [SwaggerResponse(200, "Profession updated", typeof(ProfessionDto))]
        [SwaggerResponse(404, "Profession not found")]
        public async Task<ActionResult<ProfessionDto>> Update(int id, [FromBody] ProfessionUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateProfessionCommand { Id = id, ProfessionUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/professions/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete profession by ID", Description = "Fails with 400 if any character uses the profession")]
        [SwaggerResponse(204, "Profession deleted")]
        [SwaggerResponse(400, "Profession in use")]
        [SwaggerResponse(404, "Profession not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProfessionCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/professions/{id}/species/{speciesId}
        [HttpPost("{id}/species/{speciesId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a species to the profession")]
        [SwaggerResponse(204, "Species linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddSpecies(int id, int speciesId)
        {
            await _mediator.Send(new AddProfessionSpeciesCommand { ProfessionId = id, SpeciesId = speciesId });
            return NoContent();
        }

        // DELETE: /api/professions/{id}/species/{speciesId}
        [HttpDelete("{id}/species/{speciesId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a species from the profession")]
        [SwaggerResponse(204, "Species unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveSpecies(int id, int speciesId)
        {
            var result = await _mediator.Send(new RemoveProfessionSpeciesCommand { ProfessionId = id, SpeciesId = speciesId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/professions/{id}/social-classes/{socialClassId}
        [HttpPost("{id}/social-classes/{socialClassId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a social class to the profession")]
        [SwaggerResponse(204, "Social class linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddSocialClass(int id, int socialClassId)
        {
            await _mediator.Send(new AddProfessionSocialClassCommand { ProfessionId = id, SocialClassId = socialClassId });
            return NoContent();
        }

        // DELETE: /api/professions/{id}/social-classes/{socialClassId}
        [HttpDelete("{id}/social-classes/{socialClassId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a social class from the profession")]
        [SwaggerResponse(204, "Social class unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveSocialClass(int id, int socialClassId)
        {
            var result = await _mediator.Send(new RemoveProfessionSocialClassCommand { ProfessionId = id, SocialClassId = socialClassId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/professions/{id}/trade-schools/{tradeSchoolId}
        [HttpPost("{id}/trade-schools/{tradeSchoolId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a trade school to the profession")]
        [SwaggerResponse(204, "Trade school linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddTradeSchool(int id, int tradeSchoolId)
        {
            await _mediator.Send(new AddProfessionTradeSchoolCommand { ProfessionId = id, TradeSchoolId = tradeSchoolId });
            return NoContent();
        }

        // DELETE: /api/professions/{id}/trade-schools/{tradeSchoolId}
        [HttpDelete("{id}/trade-schools/{tradeSchoolId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a trade school from the profession")]
        [SwaggerResponse(204, "Trade school unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveTradeSchool(int id, int tradeSchoolId)
        {
            var result = await _mediator.Send(new RemoveProfessionTradeSchoolCommand { ProfessionId = id, TradeSchoolId = tradeSchoolId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
