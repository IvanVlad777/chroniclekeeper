using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class IndustryRepository : IIndustryRepository
    {
        private readonly ApplicationDbContext _context;

        public IndustryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Industry> CreateAsync(Industry industry, CancellationToken cancellationToken = default)
        {
            _context.Industries.Add(industry);
            await _context.SaveChangesAsync(cancellationToken);
            return industry;
        }

        public async Task<Industry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Industries
                .Include(i => i.History)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<Industry?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Industries
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<List<Industry>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Industries.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(i => i.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Industry> UpdateAsync(Industry industry, CancellationToken cancellationToken = default)
        {
            _context.Entry(industry).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return industry;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Industries
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountGuildsUsingIndustryAsync(int industryId, CancellationToken cancellationToken = default)
        {
            return await _context.Guilds
                .CountAsync(g => g.IndustryId == industryId, cancellationToken);
        }

        public async Task<int> CountCorporationsUsingIndustryAsync(int industryId, CancellationToken cancellationToken = default)
        {
            return await _context.Corporations
                .CountAsync(c => c.IndustryId == industryId, cancellationToken);
        }
    }

    public class ExtractionMethodRepository : IExtractionMethodRepository
    {
        private readonly ApplicationDbContext _context;

        public ExtractionMethodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExtractionMethod> CreateAsync(ExtractionMethod extractionMethod, CancellationToken cancellationToken = default)
        {
            _context.ExtractionMethods.Add(extractionMethod);
            await _context.SaveChangesAsync(cancellationToken);
            return extractionMethod;
        }

        public async Task<ExtractionMethod?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ExtractionMethods
                .Include(e => e.History)
                .Include(e => e.ResourcesExtracted)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<ExtractionMethod?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ExtractionMethods
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<ExtractionMethod>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.ExtractionMethods.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(e => e.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<ExtractionMethod> UpdateAsync(ExtractionMethod extractionMethod, CancellationToken cancellationToken = default)
        {
            _context.Entry(extractionMethod).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return extractionMethod;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ExtractionMethods
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountNaturalResourcesUsingExtractionMethodAsync(int extractionMethodId, CancellationToken cancellationToken = default)
        {
            return await _context.NaturalResources
                .CountAsync(n => n.ExtractionMethodId == extractionMethodId, cancellationToken);
        }
    }

    public class NaturalResourceRepository : INaturalResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public NaturalResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NaturalResource> CreateAsync(NaturalResource naturalResource, CancellationToken cancellationToken = default)
        {
            _context.NaturalResources.Add(naturalResource);
            await _context.SaveChangesAsync(cancellationToken);
            return naturalResource;
        }

        public async Task<NaturalResource?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.NaturalResources
                .Include(n => n.ExtractionMethod)
                .Include(n => n.History)
                .Include(n => n.Locations).ThenInclude(nl => nl.Location)
                .Include(n => n.ExportRoutes).ThenInclude(tr => tr.TradeRoute)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task<NaturalResource?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.NaturalResources
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task<List<NaturalResource>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.NaturalResources.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(n => n.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<NaturalResource> UpdateAsync(NaturalResource naturalResource, CancellationToken cancellationToken = default)
        {
            _context.Entry(naturalResource).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return naturalResource;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.NaturalResources
                .Where(n => n.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsLocationLinkedAsync(int naturalResourceId, int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.NaturalResourceLocations
                .AnyAsync(nl => nl.NaturalResourceId == naturalResourceId && nl.LocationId == locationId, cancellationToken);
        }

        public async Task AddLocationAsync(int naturalResourceId, int locationId, CancellationToken cancellationToken = default)
        {
            _context.NaturalResourceLocations.Add(new NaturalResourceLocation { NaturalResourceId = naturalResourceId, LocationId = locationId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveLocationAsync(int naturalResourceId, int locationId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.NaturalResourceLocations
                .Where(nl => nl.NaturalResourceId == naturalResourceId && nl.LocationId == locationId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }

    public class TradeRouteRepository : ITradeRouteRepository
    {
        private readonly ApplicationDbContext _context;

        public TradeRouteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TradeRoute> CreateAsync(TradeRoute tradeRoute, CancellationToken cancellationToken = default)
        {
            _context.TradeRoutes.Add(tradeRoute);
            await _context.SaveChangesAsync(cancellationToken);
            return tradeRoute;
        }

        public async Task<TradeRoute?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.TradeRoutes
                .Include(t => t.History)
                .Include(t => t.Locations).ThenInclude(tl => tl.Location)
                .Include(t => t.ResourcesTraded).ThenInclude(tr => tr.NaturalResource)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<TradeRoute?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.TradeRoutes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TradeRoute>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.TradeRoutes.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(t => t.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TradeRoute> UpdateAsync(TradeRoute tradeRoute, CancellationToken cancellationToken = default)
        {
            _context.Entry(tradeRoute).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return tradeRoute;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.TradeRoutes
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsLocationLinkedAsync(int tradeRouteId, int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.TradeRouteLocations
                .AnyAsync(tl => tl.TradeRouteId == tradeRouteId && tl.LocationId == locationId, cancellationToken);
        }

        public async Task AddLocationAsync(int tradeRouteId, int locationId, CancellationToken cancellationToken = default)
        {
            _context.TradeRouteLocations.Add(new TradeRouteLocation { TradeRouteId = tradeRouteId, LocationId = locationId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveLocationAsync(int tradeRouteId, int locationId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.TradeRouteLocations
                .Where(tl => tl.TradeRouteId == tradeRouteId && tl.LocationId == locationId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsResourceLinkedAsync(int tradeRouteId, int naturalResourceId, CancellationToken cancellationToken = default)
        {
            return await _context.TradeRouteResources
                .AnyAsync(tr => tr.TradeRouteId == tradeRouteId && tr.NaturalResourceId == naturalResourceId, cancellationToken);
        }

        public async Task AddResourceAsync(int tradeRouteId, int naturalResourceId, CancellationToken cancellationToken = default)
        {
            _context.TradeRouteResources.Add(new TradeRouteResource { TradeRouteId = tradeRouteId, NaturalResourceId = naturalResourceId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveResourceAsync(int tradeRouteId, int naturalResourceId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.TradeRouteResources
                .Where(tr => tr.TradeRouteId == tradeRouteId && tr.NaturalResourceId == naturalResourceId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
