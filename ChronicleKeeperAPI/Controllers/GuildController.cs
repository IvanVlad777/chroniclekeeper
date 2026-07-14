using ChronicleKeeper.Core.CQRS.GuildRanks.Commands;
using ChronicleKeeper.Core.CQRS.GuildRanks.Queries;
using ChronicleKeeper.Core.CQRS.Guilds.Commands;
using ChronicleKeeper.Core.CQRS.Guilds.Queries;
using ChronicleKeeper.Core.DTOs.Guild;
using ChronicleKeeper.Core.DTOs.GuildRank;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/guilds")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GuildController> _logger;

        public GuildController(IMediator mediator, ILogger<GuildController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get guilds", Description = "Returns guilds, optionally filtered by world")]
        [SwaggerResponse(200, "List of guilds", typeof(IEnumerable<GuildDto>))]
        public async Task<ActionResult<IEnumerable<GuildDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllGuildsQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get guild by ID", Description = "Returns guild with its systems, ranks and cross-links")]
        [SwaggerResponse(200, "Guild found", typeof(GuildDetailsDto))]
        [SwaggerResponse(404, "Guild not found")]
        public async Task<ActionResult<GuildDetailsDto>> GetById(int id)
        {
            var guild = await _mediator.Send(new GetGuildByIdQuery { Id = id });
            if (guild == null) return NotFound();
            return Ok(guild);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new guild")]
        [SwaggerResponse(201, "Guild created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<GuildDto>> Create([FromBody] GuildCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating guild: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateGuildCommand { GuildCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update guild by ID")]
        [SwaggerResponse(200, "Guild updated", typeof(GuildDto))]
        [SwaggerResponse(404, "Guild not found")]
        public async Task<ActionResult<GuildDto>> Update(int id, [FromBody] GuildUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateGuildCommand { Id = id, GuildUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete guild by ID")]
        [SwaggerResponse(204, "Guild deleted")]
        [SwaggerResponse(404, "Guild not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGuildCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/guilds/{id}/factions/{factionId}
        [HttpPost("{id}/factions/{factionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a faction to the guild")]
        [SwaggerResponse(204, "Faction linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddFaction(int id, int factionId)
        {
            await _mediator.Send(new AddGuildFactionCommand { GuildId = id, FactionId = factionId });
            return NoContent();
        }

        // DELETE: /api/guilds/{id}/factions/{factionId}
        [HttpDelete("{id}/factions/{factionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a faction from the guild")]
        [SwaggerResponse(204, "Faction unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveFaction(int id, int factionId)
        {
            var result = await _mediator.Send(new RemoveGuildFactionCommand { GuildId = id, FactionId = factionId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/guilds/{id}/professions/{professionId}
        [HttpPost("{id}/professions/{professionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a profession to the guild")]
        [SwaggerResponse(204, "Profession linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddProfession(int id, int professionId)
        {
            await _mediator.Send(new AddGuildProfessionCommand { GuildId = id, ProfessionId = professionId });
            return NoContent();
        }

        // DELETE: /api/guilds/{id}/professions/{professionId}
        [HttpDelete("{id}/professions/{professionId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a profession from the guild")]
        [SwaggerResponse(204, "Profession unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveProfession(int id, int professionId)
        {
            var result = await _mediator.Send(new RemoveGuildProfessionCommand { GuildId = id, ProfessionId = professionId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/guilds/{id}/social-classes/{socialClassId}
        [HttpPost("{id}/social-classes/{socialClassId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a social class to the guild")]
        [SwaggerResponse(204, "Social class linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddSocialClass(int id, int socialClassId)
        {
            await _mediator.Send(new AddGuildSocialClassCommand { GuildId = id, SocialClassId = socialClassId });
            return NoContent();
        }

        // DELETE: /api/guilds/{id}/social-classes/{socialClassId}
        [HttpDelete("{id}/social-classes/{socialClassId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a social class from the guild")]
        [SwaggerResponse(204, "Social class unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveSocialClass(int id, int socialClassId)
        {
            var result = await _mediator.Send(new RemoveGuildSocialClassCommand { GuildId = id, SocialClassId = socialClassId });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/guild-ranks")]
    [ApiController]
    public class GuildRankController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildRankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get guild ranks", Description = "Returns guild ranks, optionally filtered by world and/or guild")]
        [SwaggerResponse(200, "List of guild ranks", typeof(IEnumerable<GuildRankDto>))]
        public async Task<ActionResult<IEnumerable<GuildRankDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? guildId = null)
        {
            return Ok(await _mediator.Send(new GetAllGuildRanksQuery { WorldId = worldId, GuildId = guildId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get guild rank by ID")]
        [SwaggerResponse(200, "Guild rank found", typeof(GuildRankDto))]
        [SwaggerResponse(404, "Guild rank not found")]
        public async Task<ActionResult<GuildRankDto>> GetById(int id)
        {
            var guildRank = await _mediator.Send(new GetGuildRankByIdQuery { Id = id });
            if (guildRank == null) return NotFound();
            return Ok(guildRank);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new guild rank", Description = "The rank's world is derived from its guild")]
        [SwaggerResponse(201, "Guild rank created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<GuildRankDto>> Create([FromBody] GuildRankCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateGuildRankCommand { GuildRankCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update guild rank by ID")]
        [SwaggerResponse(200, "Guild rank updated", typeof(GuildRankDto))]
        [SwaggerResponse(404, "Guild rank not found")]
        public async Task<ActionResult<GuildRankDto>> Update(int id, [FromBody] GuildRankUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateGuildRankCommand { Id = id, GuildRankUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete guild rank by ID")]
        [SwaggerResponse(204, "Guild rank deleted")]
        [SwaggerResponse(404, "Guild rank not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGuildRankCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
