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

        public CharacterController(ApplicationDbContext context, ILogger<CharacterController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /api/characters
        [HttpGet]
        [SwaggerOperation(Summary = "Get all characters", Description = "Returns a list of all characters")]
        [SwaggerResponse(200, "List of characters", typeof(IEnumerable<CharacterDto>))]
        public async Task<ActionResult<IEnumerable<CharacterDto>>> GetAll()
        {
            _logger.LogInformation("Fetching all characters...");

            var characters = await _context.Characters
                .AsNoTracking()
                .Select(c => new CharacterDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Nickname = c.Nickname,
                    Title = c.Title,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Height = c.Height,
                    Weight = c.Weight,
                    HairColor = c.HairColor,
                    EyeColor = c.EyeColor,
                    SpecialPhysicalFeatures = c.SpecialPhysicalFeatures,
                    IsArtificial = c.IsArtificial,
                    Species = new ReferenceDto { Id = c.SapientSpecies.Id, Name = c.SapientSpecies.Name },
                    Religion = c.Religion == null ? null : new ReferenceDto { Id = c.Religion.Id, Name = c.Religion.Name },
                    Nation = c.Nation == null ? null : new ReferenceDto { Id = c.Nation.Id, Name = c.Nation.Name },
                    Profession = c.Profession == null ? null : new ReferenceDto { Id = c.Profession.Id, Name = c.Profession.Name },
                    SocialClass = c.SocialClass == null ? null : new ReferenceDto { Id = c.SocialClass.Id, Name = c.SocialClass.Name },
                    Father = c.Father == null ? null : new ReferenceDto { Id = c.Father.Id, Name = c.Father.Name },
                    Mother = c.Mother == null ? null : new ReferenceDto { Id = c.Mother.Id, Name = c.Mother.Name }
                })
                .ToListAsync();

            _logger.LogInformation("Returned {Count} characters.", characters.Count);

            return Ok(characters);
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
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CharacterDetailsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Nickname = c.Nickname,
                    Title = c.Title,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Height = c.Height,
                    Weight = c.Weight,
                    HairColor = c.HairColor,
                    EyeColor = c.EyeColor,
                    SpecialPhysicalFeatures = c.SpecialPhysicalFeatures,
                    IsArtificial = c.IsArtificial,
                    Species = new ReferenceDto { Id = c.SapientSpecies.Id, Name = c.SapientSpecies.Name },
                    Religion = c.Religion == null ? null : new ReferenceDto { Id = c.Religion.Id, Name = c.Religion.Name },
                    Nation = c.Nation == null ? null : new ReferenceDto { Id = c.Nation.Id, Name = c.Nation.Name },
                    Profession = c.Profession == null ? null : new ReferenceDto { Id = c.Profession.Id, Name = c.Profession.Name },
                    SocialClass = c.SocialClass == null ? null : new ReferenceDto { Id = c.SocialClass.Id, Name = c.SocialClass.Name },
                    Father = c.Father == null ? null : new ReferenceDto { Id = c.Father.Id, Name = c.Father.Name },
                    Mother = c.Mother == null ? null : new ReferenceDto { Id = c.Mother.Id, Name = c.Mother.Name },
                    Abilities = c.Abilities.Select(a => new ReferenceDto { Id = a.Id, Name = a.Name }).ToList(),
                    Hobbies = c.Hobbies.Select(h => new ReferenceDto { Id = h.Id, Name = h.Name }).ToList(),
                    Equipments = c.Equipments.Select(e => new ReferenceDto { Id = e.Id, Name = e.Name }).ToList(),
                    Clothing = c.Clothing.Select(cl => new ReferenceDto { Id = cl.Id, Name = cl.Name }).ToList(),
                    Educations = c.Educations.Select(ed => new ReferenceDto { Id = ed.Id, Name = ed.Name }).ToList(),
                    Specialisations = c.Specialisations.Select(s => new ReferenceDto { Id = s.Id, Name = s.Name }).ToList(),
                    Factions = c.Factions.Select(f => new ReferenceDto { Id = f.Id, Name = f.Name }).ToList(),
                    Siblings = c.Siblings.Select(sib => new ReferenceDto { Id = sib.Id, Name = sib.Name }).ToList()
                })
                .FirstOrDefaultAsync();

            if (character == null)
            {
                _logger.LogWarning("Character with ID {Id} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Returned character with ID {Id}.", id);
            return Ok(character);
        }

        // POST: /api/characters
        [HttpPost]
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

            var character = new Character
            {
                Name = dto.Name,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Nickname = dto.Nickname,
                Title = dto.Title,
                BirthDate = dto.BirthDate,
                IsArtificial = dto.IsArtificial,
                SapientSpeciesId = dto.SapientSpeciesId,
                NationId = dto.NationId,
                ReligionId = dto.ReligionId,
                ProfessionId = dto.ProfessionId,
                SocialClassId = dto.SocialClassId,
                FatherId = dto.FatherId,
                MotherId = dto.MotherId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created character with ID {Id}", character.Id);
            return CreatedAtAction(nameof(GetById), new { id = character.Id }, null);
        }

        // DELETE: /api/characters/{id}
        [HttpDelete("{id}")]
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
