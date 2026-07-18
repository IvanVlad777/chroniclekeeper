using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Location> CreateAsync(Location location, CancellationToken cancellationToken = default)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync(cancellationToken);
            return location;
        }

        public async Task<Location?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // _context.Locations (base DbSet) returns any TPH subtype (Continent/Region/Country/City/District) — namjerno.
            return await _context.Locations
                .Include(l => l.ParentLocation)
                .Include(l => l.SubLocations)
                .Include(l => l.Tags).ThenInclude(t => t.Tag)
                .Include(l => l.History)
                .Include(l => l.Schools)
                .Include(l => ((Country)l).GovernmentSystem)
                .Include(l => ((Country)l).LegalSystem)
                .Include(l => ((Country)l).EducationSystem)
                .Include(l => ((Country)l).EconomicSystem)
                .Include(l => ((City)l).GovernmentSystem)
                .Include(l => ((City)l).LegalSystem)
                .Include(l => ((City)l).EducationSystem)
                .Include(l => ((City)l).EconomicSystem)
                .Include(l => ((Region)l).OriginOfSapientSpecies).ThenInclude(rs => rs.SapientSpecies)
                .Include(l => ((RiverEcosystem)l).SourceLocation)
                .Include(l => ((RiverEcosystem)l).MouthLocation)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<Location?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<List<Location>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Locations.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(l => l.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Location> UpdateAsync(Location location, CancellationToken cancellationToken = default)
        {
            _context.Entry(location).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return location;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // LocationTags kaskadira DB; Faction.HeadquartersId je SET NULL;
            // djeca su provjerena u handleru (Restrict)
            var deleted = await _context.Locations
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> ExistsInWorldAsync(int locationId, int worldId, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .AnyAsync(l => l.Id == locationId && l.WorldId == worldId, cancellationToken);
        }

        public async Task<bool> HasChildrenAsync(int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .AnyAsync(l => l.ParentLocationId == locationId, cancellationToken);
        }

        public async Task<bool> IsReferencedAsRiverEndpointAsync(int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RiverEcosystem>()
                .AnyAsync(r => r.SourceLocationId == locationId || r.MouthLocationId == locationId, cancellationToken);
        }

        public async Task<bool> IsReferencedByHolySiteAsync(int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<HolySite>()
                .AnyAsync(h => h.LocationId == locationId, cancellationToken);
        }

        public async Task<bool> WouldCreateCycleAsync(int locationId, int newParentId, CancellationToken cancellationToken = default)
        {
            // Hodaj uzlazno od novog roditelja; ako lanac dođe do same lokacije — ciklus
            int? current = newParentId;
            var visited = new HashSet<int>();
            while (current is int cid)
            {
                if (cid == locationId) return true;
                if (!visited.Add(cid)) return true; // postojeći ciklus u podacima — ne pogoršavaj

                current = await _context.Locations
                    .Where(l => l.Id == cid)
                    .Select(l => l.ParentLocationId)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            return false;
        }

        public async Task<bool> IsNativeSpeciesLinkedAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            return await _context.RegionSapientSpecies
                .AnyAsync(rs => rs.RegionId == regionId && rs.SapientSpeciesId == sapientSpeciesId, cancellationToken);
        }

        public async Task AddNativeSpeciesAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            _context.RegionSapientSpecies.Add(new RegionSapientSpecies { RegionId = regionId, SapientSpeciesId = sapientSpeciesId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveNativeSpeciesAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.RegionSapientSpecies
                .Where(rs => rs.RegionId == regionId && rs.SapientSpeciesId == sapientSpeciesId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
