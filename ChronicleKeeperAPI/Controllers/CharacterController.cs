using AutoMapper;
using ChronicleKeeper.Core.CQRS.Characters.Commands;
using ChronicleKeeper.Core.CQRS.Characters.Queries;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Character;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/characters")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CharacterController> _logger;

        public CharacterController(IMediator mediator, ILogger<CharacterController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/characters?worldId=1
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get characters", Description = "Returns characters, optionally filtered by world")]
        [SwaggerResponse(200, "List of characters", typeof(IEnumerable<CharacterDto>))]
        public async Task<ActionResult<IEnumerable<CharacterDto>>> GetAll([FromQuery] int? worldId = null)
        {
            _logger.LogInformation("API: Fetching characters (worldId: {WorldId})", worldId);
            var query = new GetAllCharactersQuery { WorldId = worldId };
            var characters = await _mediator.Send(query);
            _logger.LogInformation("API: Returned {Count} characters", characters.Count);
            return Ok(characters);
        }

        // GET: /api/characters/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get character by ID", Description = "Returns detailed character info")]
        [SwaggerResponse(200, "Character found", typeof(CharacterDto))]
        [SwaggerResponse(404, "Character not found")]
        public async Task<ActionResult<CharacterDto>> GetById(int id)
        {
            _logger.LogInformation("API: Fetching character with ID {Id}", id);
            var query = new GetCharacterByIdQuery { Id = id };
            var character = await _mediator.Send(query);
            
            if (character == null)
            {
                _logger.LogWarning("API: Character with ID {Id} not found", id);
                return NotFound();
            }
            
            _logger.LogInformation("API: Returned character with ID {Id}", id);
            return Ok(character);
        }

        // POST: /api/characters
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new character", Description = "Creates a new character entry")]
        [SwaggerResponse(201, "Character created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<CharacterDto>> Create([FromBody] CharacterCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("API: Invalid character creation request");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("API: Creating character: {Name}", dto.Name);
            var command = new CreateCharacterCommand { CharacterCreateDto = dto };
            var result = await _mediator.Send(command);
            _logger.LogInformation("API: Created character with ID {Id}", result.Id);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/characters/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update character by ID", Description = "Updates an existing character entry")]
        [SwaggerResponse(200, "Character updated", typeof(CharacterDto))]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "Character not found")]
        public async Task<ActionResult<CharacterDto>> Update(int id, [FromBody] CharacterUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("API: Invalid character update request for ID {Id}", id);
                return BadRequest(ModelState);
            }

            // Missing id → EntityNotFoundException → 404 kroz GlobalExceptionMiddleware
            _logger.LogInformation("API: Updating character with ID {Id}", id);
            var command = new UpdateCharacterCommand { Id = id, CharacterUpdateDto = dto };
            var result = await _mediator.Send(command);
            _logger.LogInformation("API: Updated character with ID {Id}", id);
            return Ok(result);
        }

        // POST: /api/characters/{id}/relationships
        [HttpPost("{id}/relationships")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Add relationship to character", Description = "Related character must belong to the same world")]
        [SwaggerResponse(201, "Relationship created", typeof(CharacterRelationshipDto))]
        [SwaggerResponse(400, "Invalid input / duplicate")]
        [SwaggerResponse(404, "Character not found")]
        public async Task<ActionResult<CharacterRelationshipDto>> AddRelationship(int id, [FromBody] CharacterRelationshipCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new AddCharacterRelationshipCommand { CharacterId = id, RelationshipDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, result);
        }

        // DELETE: /api/characters/{id}/relationships/{relationshipId}
        [HttpDelete("{id}/relationships/{relationshipId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Remove relationship from character")]
        [SwaggerResponse(204, "Relationship removed")]
        [SwaggerResponse(404, "Relationship not found")]
        public async Task<IActionResult> RemoveRelationship(int id, int relationshipId)
        {
            var result = await _mediator.Send(new RemoveCharacterRelationshipCommand { CharacterId = id, RelationshipId = relationshipId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/characters/{id}/abilities/{abilityId}
        [HttpPost("{id}/abilities/{abilityId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link an ability to the character")]
        [SwaggerResponse(204, "Ability linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddAbility(int id, int abilityId)
        {
            await _mediator.Send(new AddCharacterAbilityCommand { CharacterId = id, AbilityId = abilityId });
            return NoContent();
        }

        // DELETE: /api/characters/{id}/abilities/{abilityId}
        [HttpDelete("{id}/abilities/{abilityId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink an ability from the character")]
        [SwaggerResponse(204, "Ability unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveAbility(int id, int abilityId)
        {
            var result = await _mediator.Send(new RemoveCharacterAbilityCommand { CharacterId = id, AbilityId = abilityId });
            if (!result) return NotFound();
            return NoContent();
        }

        // DELETE: /api/characters/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete character by ID", Description = "Deletes a character entry")]
        [SwaggerResponse(204, "Character deleted")]
        [SwaggerResponse(404, "Character not found")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("API: Deleting character with ID {Id}", id);
            var command = new DeleteCharacterCommand { Id = id };
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                _logger.LogWarning("API: Character with ID {Id} not found for deletion", id);
                return NotFound();
            }
            
            _logger.LogInformation("API: Deleted character with ID {Id}", id);
            return NoContent();
        }
    }
}
