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
                .Include(c => c.Habitants).ThenInclude(ce => ce.Ecosystem)
                .Include(c => c.SymbioticPartners).ThenInclude(x => x.SymbioticPartner)
                .Include(c => c.Prey).ThenInclude(x => x.Prey)
                .Include(c => c.Predators).ThenInclude(x => x.Predator)
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
            // SapientSpecies shares the Creatures TPH table but has its own dedicated vertical
            // (api/species) — keep it out of the generic creature list so the Creature UI stays clean.
            var query = _context.Creatures.AsNoTracking()
                .Where(c => EF.Property<string>(c, "CreatureSubtype") != "Sapient");
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
            // Clear the Restrict sides of the self-ref joins first (the Cascade sides — CreatureId /
            // PredatorCreatureId — clean themselves when the row is deleted).
            await _context.CreatureSymbioses.Where(x => x.SymbioticPartnerId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.CreaturePredations.Where(x => x.PreyCreatureId == id).ExecuteDeleteAsync(cancellationToken);

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

        public async Task<bool> IsHabitatLinkedAsync(int creatureId, int ecosystemId, CancellationToken cancellationToken = default)
        {
            return await _context.CreatureEcosystems
                .AnyAsync(ce => ce.CreatureId == creatureId && ce.EcosystemId == ecosystemId, cancellationToken);
        }

        public async Task AddHabitatAsync(int creatureId, int ecosystemId, CancellationToken cancellationToken = default)
        {
            _context.CreatureEcosystems.Add(new CreatureEcosystem { CreatureId = creatureId, EcosystemId = ecosystemId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveHabitatAsync(int creatureId, int ecosystemId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CreatureEcosystems
                .Where(ce => ce.CreatureId == creatureId && ce.EcosystemId == ecosystemId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsSymbioticLinkedAsync(int creatureId, int partnerId, CancellationToken cancellationToken = default)
        {
            return await _context.CreatureSymbioses
                .AnyAsync(x => x.CreatureId == creatureId && x.SymbioticPartnerId == partnerId, cancellationToken);
        }

        public async Task AddSymbiosisAsync(int creatureId, int partnerId, CancellationToken cancellationToken = default)
        {
            _context.CreatureSymbioses.Add(new CreatureSymbiosis { CreatureId = creatureId, SymbioticPartnerId = partnerId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveSymbiosisAsync(int creatureId, int partnerId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CreatureSymbioses
                .Where(x => x.CreatureId == creatureId && x.SymbioticPartnerId == partnerId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsPreyLinkedAsync(int predatorId, int preyId, CancellationToken cancellationToken = default)
        {
            return await _context.CreaturePredations
                .AnyAsync(x => x.PredatorCreatureId == predatorId && x.PreyCreatureId == preyId, cancellationToken);
        }

        public async Task AddPreyAsync(int predatorId, int preyId, CancellationToken cancellationToken = default)
        {
            _context.CreaturePredations.Add(new CreaturePredation { PredatorCreatureId = predatorId, PreyCreatureId = preyId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemovePreyAsync(int predatorId, int preyId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CreaturePredations
                .Where(x => x.PredatorCreatureId == predatorId && x.PreyCreatureId == preyId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
