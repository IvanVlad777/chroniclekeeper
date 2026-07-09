using ChronicleKeeper.Core.CQRS.Languages.Commands;
using ChronicleKeeper.Core.CQRS.Languages.Queries;
using ChronicleKeeper.Core.DTOs.Language;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LanguageController> _logger;

        public LanguageController(IMediator mediator, ILogger<LanguageController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/languages?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get languages", Description = "Returns languages, optionally filtered by world")]
        [SwaggerResponse(200, "List of languages", typeof(IEnumerable<LanguageDto>))]
        public async Task<ActionResult<IEnumerable<LanguageDto>>> GetAll([FromQuery] int? worldId = null)
        {
            var languages = await _mediator.Send(new GetAllLanguagesQuery { WorldId = worldId });
            return Ok(languages);
        }

        // GET: /api/languages/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get language by ID", Description = "Returns language with its cultures")]
        [SwaggerResponse(200, "Language found", typeof(LanguageDetailsDto))]
        [SwaggerResponse(404, "Language not found")]
        public async Task<ActionResult<LanguageDetailsDto>> GetById(int id)
        {
            var language = await _mediator.Send(new GetLanguageByIdQuery { Id = id });
            if (language == null) return NotFound();
            return Ok(language);
        }

        // POST: /api/languages
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new language")]
        [SwaggerResponse(201, "Language created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<LanguageDto>> Create([FromBody] LanguageCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating language: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateLanguageCommand { LanguageCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/languages/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update language by ID")]
        [SwaggerResponse(200, "Language updated", typeof(LanguageDto))]
        [SwaggerResponse(404, "Language not found")]
        public async Task<ActionResult<LanguageDto>> Update(int id, [FromBody] LanguageUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateLanguageCommand { Id = id, LanguageUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/languages/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete language by ID", Description = "Fails with 400 if any culture uses the language")]
        [SwaggerResponse(204, "Language deleted")]
        [SwaggerResponse(400, "Language in use")]
        [SwaggerResponse(404, "Language not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteLanguageCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
