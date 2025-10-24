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

        // GET: /api/characters
        [HttpGet]
        [SwaggerOperation(Summary = "Get all characters", Description = "Returns a list of all characters")]
        [SwaggerResponse(200, "List of characters", typeof(IEnumerable<CharacterDto>))]
        public async Task<ActionResult<IEnumerable<CharacterDto>>> GetAll()
        {
            _logger.LogInformation("API: Fetching all characters");
            var query = new GetAllCharactersQuery();
            var characters = await _mediator.Send(query);
            _logger.LogInformation("API: Returned {Count} characters", characters.Count);
            return Ok(characters);
        }

        // GET: /api/characters/{id}
        [HttpGet("{id}")]
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

            try
            {
                _logger.LogInformation("API: Updating character with ID {Id}", id);
                var command = new UpdateCharacterCommand { Id = id, CharacterUpdateDto = dto };
                var result = await _mediator.Send(command);
                _logger.LogInformation("API: Updated character with ID {Id}", id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("API: Character with ID {Id} not found for update", id);
                return NotFound(ex.Message);
            }
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
