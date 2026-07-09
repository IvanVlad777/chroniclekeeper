using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Repositories;
using static ChronicleKeeper.Core.Enums.LoreEnums;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ApplicationDbContext _context;

        public CharacterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Character> CreateAsync(Character character, CancellationToken cancellationToken = default)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync(cancellationToken);
            return character;
        }

        public async Task<Character?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Characters
                .Include(c => c.Father)
                .Include(c => c.Mother)
                .Include(c => c.SapientSpecies)
                .Include(c => c.Race)
                .Include(c => c.SocialClass)
                .Include(c => c.Nation)
                .Include(c => c.Religion)
                .Include(c => c.Relationships).ThenInclude(r => r.RelatedCharacter)
                .Include(c => c.Memberships).ThenInclude(m => m.Faction)
                .Include(c => c.Tags).ThenInclude(t => t.Tag)
                // TODO: Otkomentirati kada budu oživljeni
                //.Include(c => c.Profession)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Character?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Characters
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Character>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Characters.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Character> UpdateAsync(Character character, CancellationToken cancellationToken = default)
        {
            // Označi samo korijenski entitet kao izmijenjen
            _context.Entry(character).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return character;
        }

        public async Task<bool> ExistsInWorldAsync(int characterId, int worldId, CancellationToken cancellationToken = default)
        {
            return await _context.Characters
                .AnyAsync(c => c.Id == characterId && c.WorldId == worldId, cancellationToken);
        }

        public async Task<bool> SpeciesExistsInWorldAsync(int speciesId, int worldId, CancellationToken cancellationToken = default)
        {
            return await _context.SapientSpecies
                .AnyAsync(s => s.Id == speciesId && s.WorldId == worldId, cancellationToken);
        }

        public async Task<int?> GetSpeciesIdForRaceAsync(int raceId, int worldId, CancellationToken cancellationToken = default)
        {
            return await _context.Races
                .Where(r => r.Id == raceId && r.WorldId == worldId)
                .Select(r => (int?)r.SapientSpeciesId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<CharacterRelationship> AddRelationshipAsync(CharacterRelationship relationship, CancellationToken cancellationToken = default)
        {
            _context.CharacterRelationships.Add(relationship);
            await _context.SaveChangesAsync(cancellationToken);

            // Učitaj drugu stranu za mapiranje imena u DTO
            await _context.Entry(relationship).Reference(r => r.RelatedCharacter).LoadAsync(cancellationToken);
            return relationship;
        }

        public async Task<bool> RemoveRelationshipAsync(int characterId, int relationshipId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CharacterRelationships
                .Where(r => r.Id == relationshipId && r.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> RelationshipExistsAsync(int characterId, int relatedCharacterId, RelationshipType type, CancellationToken cancellationToken = default)
        {
            return await _context.CharacterRelationships
                .AnyAsync(r => r.CharacterId == characterId
                    && r.RelatedCharacterId == relatedCharacterId
                    && r.Type == type, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            // Self-ref FK-ovi (Restrict): djeca ostaju, samo gube vezu na roditelja
            await _context.Characters
                .Where(c => c.FatherId == id || c.MotherId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.FatherId, c => c.FatherId == id ? null : c.FatherId)
                    .SetProperty(c => c.MotherId, c => c.MotherId == id ? null : c.MotherId), cancellationToken);

            // Veze u kojima je ovaj lik "druga strana" (RelatedCharacterId je Restrict)
            await _context.CharacterRelationships
                .Where(r => r.RelatedCharacterId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // Sam lik — DB kaskadira Relationships (vlasnička strana), FactionMembers,
            // CharacterTags; SET NULL na Faction.LeaderId
            var deleted = await _context.Characters
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
