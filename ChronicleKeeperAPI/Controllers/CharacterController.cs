using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Character;
using ChronicleKeeper.Core.DTOs.Character.ChronicleKeeper.API.Dtos;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Infrastructure.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CharacterController> _logger;
        private readonly IMapper _mapper;

        public CharacterController(ApplicationDbContext context, ILogger<CharacterController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: /api/characters
        [HttpGet]
        [SwaggerOperation(Summary = "Get all characters", Description = "Returns a list of all characters")]
        [SwaggerResponse(200, "List of characters", typeof(IEnumerable<CharacterDto>))]
        public async Task<ActionResult<IEnumerable<CharacterDto>>> GetAll()
        {
            _logger.LogInformation("Fetching all characters...");

            var characters = await _context.Characters
                .Include(c => c.SapientSpecies)
                .Include(c => c.Religion)
                .Include(c => c.Nation)
                .Include(c => c.Profession)
                .Include(c => c.SocialClass)
                .Include(c => c.Father)
                .Include(c => c.Mother)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = _mapper.Map<List<CharacterDto>>(characters);
            _logger.LogInformation("Returned {Count} characters.", characters.Count);

            return Ok(dtoList);
        }

        // GET: /api/characters/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get character by ID", Description = "Returns detailed character info")]
        [SwaggerResponse(200, "Character found", typeof(CharacterDetailsDto))]
        [SwaggerResponse(404, "Character not found")]
        public async Task<ActionResult<CharacterDetailsDto>> GetById(int id)
        {
            _logger.LogInformation("Fetching character with ID {Id}", id);

            var character = await _context.Characters
                .Include(c => c.SapientSpecies)
                .Include(c => c.Religion)
                .Include(c => c.Nation)
                .Include(c => c.Profession)
                .Include(c => c.SocialClass)
                .Include(c => c.Father)
                .Include(c => c.Mother)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                _logger.LogWarning("Character with ID {Id} not found.", id);
                return NotFound();
            }

            var dto = _mapper.Map<CharacterDto>(character);
            _logger.LogInformation("Returned character with ID {Id}.", id);
            return Ok(dto);
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
                _logger.LogWarning("Invalid character creation request.");
                return BadRequest(ModelState);
            }

            var character = _mapper.Map<Character>(dto);
            character.CreatedAt = DateTime.UtcNow;
            character.UpdatedAt = DateTime.UtcNow;

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<CharacterDto>(character);

            _logger.LogInformation("Created character with ID {Id}", character.Id);
            return CreatedAtAction(nameof(GetById), new { id = character.Id }, resultDto);
        }

        // DELETE: /api/characters/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete character by ID", Description = "Deletes a character entry")]
        [SwaggerResponse(204, "Character deleted")]
        [SwaggerResponse(404, "Character not found")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting character with ID {Id}", id);

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                _logger.LogWarning("Attempted to delete non-existing character with ID {Id}", id);
                return NotFound();
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted character with ID {Id}", id);
            return NoContent();
        }
    }
}
