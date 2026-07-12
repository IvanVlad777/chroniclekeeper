using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class CreatureRepository : ICreatureRepository
    {
        private readonly ApplicationDbContext _context;

        public CreatureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Creature> CreateAsync(Creature creature, CancellationToken cancellationToken = default)
        {
            _context.Creatures.Add(creature);
            await _context.SaveChangesAsync(cancellationToken);
            return creature;
        }

        public async Task<Creature?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Creatures
                .Include(c => c.ParentCreature)
                .Include(c => c.Subspecies)
                .Include(c => c.History)
                .Include(c => c.CitiesItInhabits).ThenInclude(cc => cc.City)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Creature?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Creatures
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Creature>> GetAllAsync(int? worldId = null, string? subtype = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Creatures.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            if (!string.IsNullOrWhiteSpace(subtype))
            {
                // "CreatureSubtype" is the shadow discriminator property configured in CreatureConfiguration.
                query = query.Where(c => EF.Property<string>(c, "CreatureSubtype") == subtype);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Creature> UpdateAsync(Creature creature, CancellationToken cancellationToken = default)
        {
            _context.Entry(creature).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return creature;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Creatures
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> HasSubspeciesAsync(int creatureId, CancellationToken cancellationToken = default)
        {
            return await _context.Creatures
                .AnyAsync(c => c.ParentCreatureId == creatureId, cancellationToken);
        }

        public async Task<bool> WouldCreateCycleAsync(int creatureId, int newParentId, CancellationToken cancellationToken = default)
        {
            int? current = newParentId;
            var visited = new HashSet<int>();
            while (current is int cid)
            {
                if (cid == creatureId) return true;
                if (!visited.Add(cid)) return true; // postojeći ciklus u podacima — ne pogoršavaj

                current = await _context.Creatures
                    .Where(c => c.Id == cid)
                    .Select(c => c.ParentCreatureId)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            return false;
        }

        public async Task<bool> IsCityLinkedAsync(int creatureId, int cityId, CancellationToken cancellationToken = default)
        {
            return await _context.CreatureCities
                .AnyAsync(cc => cc.CreatureId == creatureId && cc.CityId == cityId, cancellationToken);
        }

        public async Task AddCityAsync(int creatureId, int cityId, CancellationToken cancellationToken = default)
        {
            _context.CreatureCities.Add(new CreatureCity { CreatureId = creatureId, CityId = cityId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveCityAsync(int creatureId, int cityId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CreatureCities
                .Where(cc => cc.CreatureId == creatureId && cc.CityId == cityId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
