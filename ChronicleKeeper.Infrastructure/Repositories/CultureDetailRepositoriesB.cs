using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ArchitectureStyleRepository : IArchitectureStyleRepository
    {
        private readonly ApplicationDbContext _context;
        public ArchitectureStyleRepository(ApplicationDbContext context) => _context = context;

        public async Task<ArchitectureStyle> CreateAsync(ArchitectureStyle entity, CancellationToken ct = default)
        {
            _context.ArchitectureStyles.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<ArchitectureStyle?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.ArchitectureStyles
                .Include(a => a.History)
                .Include(a => a.Culture)
                .Include(a => a.TypicalLocations).ThenInclude(al => al.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, ct);

        public Task<ArchitectureStyle?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.ArchitectureStyles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

        public async Task<List<ArchitectureStyle>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.ArchitectureStyles.AsNoTracking();
            if (worldId is int w) q = q.Where(a => a.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<ArchitectureStyle> UpdateAsync(ArchitectureStyle entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.ArchitectureStyles.Where(a => a.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> LocationExistsInWorldAsync(int locationId, int worldId, CancellationToken ct = default) =>
            _context.Locations.AnyAsync(l => l.Id == locationId && l.WorldId == worldId, ct);

        public async Task AddLocationAsync(int styleId, int locationId, CancellationToken ct = default)
        {
            if (!await _context.ArchitectureStyleLocations.AnyAsync(x => x.ArchitectureStyleId == styleId && x.LocationId == locationId, ct))
            {
                _context.ArchitectureStyleLocations.Add(new ArchitectureStyleLocation { ArchitectureStyleId = styleId, LocationId = locationId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveLocationAsync(int styleId, int locationId, CancellationToken ct = default) =>
            await _context.ArchitectureStyleLocations
                .Where(x => x.ArchitectureStyleId == styleId && x.LocationId == locationId)
                .ExecuteDeleteAsync(ct);
    }

    public class FolkloreRepository : IFolkloreRepository
    {
        private readonly ApplicationDbContext _context;
        public FolkloreRepository(ApplicationDbContext context) => _context = context;

        public async Task<Folklore> CreateAsync(Folklore entity, CancellationToken ct = default)
        {
            _context.Folktales.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Folklore?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Folktales
                .Include(f => f.History)
                .Include(f => f.Culture)
                .Include(f => f.RelatedEvents).ThenInclude(fe => fe.TimelineEvent)
                .Include(f => f.OriginatedFromSpecies).ThenInclude(fs => fs.SapientSpecies)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, ct);

        public Task<Folklore?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Folktales.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);

        public async Task<List<Folklore>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Folktales.AsNoTracking();
            if (worldId is int w) q = q.Where(f => f.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Folklore> UpdateAsync(Folklore entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Folktales.Where(f => f.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> EventExistsInWorldAsync(int eventId, int worldId, CancellationToken ct = default) =>
            _context.TimelineEvents.AnyAsync(e => e.Id == eventId && e.WorldId == worldId, ct);

        public async Task AddEventAsync(int folkloreId, int eventId, CancellationToken ct = default)
        {
            if (!await _context.FolkloreTimelineEvents.AnyAsync(x => x.FolkloreId == folkloreId && x.TimelineEventId == eventId, ct))
            {
                _context.FolkloreTimelineEvents.Add(new FolkloreTimelineEvent { FolkloreId = folkloreId, TimelineEventId = eventId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveEventAsync(int folkloreId, int eventId, CancellationToken ct = default) =>
            await _context.FolkloreTimelineEvents
                .Where(x => x.FolkloreId == folkloreId && x.TimelineEventId == eventId)
                .ExecuteDeleteAsync(ct);

        public Task<bool> SpeciesExistsInWorldAsync(int speciesId, int worldId, CancellationToken ct = default) =>
            _context.SapientSpecies.AnyAsync(s => s.Id == speciesId && s.WorldId == worldId, ct);

        public async Task AddSpeciesAsync(int folkloreId, int speciesId, CancellationToken ct = default)
        {
            if (!await _context.FolkloreSapientSpecies.AnyAsync(x => x.FolkloreId == folkloreId && x.SapientSpeciesId == speciesId, ct))
            {
                _context.FolkloreSapientSpecies.Add(new FolkloreSapientSpecies { FolkloreId = folkloreId, SapientSpeciesId = speciesId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveSpeciesAsync(int folkloreId, int speciesId, CancellationToken ct = default) =>
            await _context.FolkloreSapientSpecies
                .Where(x => x.FolkloreId == folkloreId && x.SapientSpeciesId == speciesId)
                .ExecuteDeleteAsync(ct);
    }

    public class MythRepository : IMythRepository
    {
        private readonly ApplicationDbContext _context;
        public MythRepository(ApplicationDbContext context) => _context = context;

        public async Task<Myth> CreateAsync(Myth entity, CancellationToken ct = default)
        {
            _context.Myths.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Myth?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Myths
                .Include(m => m.History)
                .Include(m => m.Culture)
                .Include(m => m.Religion)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, ct);

        public Task<Myth?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Myths.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, ct);

        public async Task<List<Myth>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Myths.AsNoTracking();
            if (worldId is int w) q = q.Where(m => m.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Myth> UpdateAsync(Myth entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Myths.Where(m => m.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class CulturalFestivalRepository : ICulturalFestivalRepository
    {
        private readonly ApplicationDbContext _context;
        public CulturalFestivalRepository(ApplicationDbContext context) => _context = context;

        public async Task<CulturalFestival> CreateAsync(CulturalFestival entity, CancellationToken ct = default)
        {
            _context.CulturalFestivals.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<CulturalFestival?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.CulturalFestivals
                .Include(f => f.History)
                .Include(f => f.Culture)
                .Include(f => f.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, ct);

        public Task<CulturalFestival?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.CulturalFestivals.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);

        public async Task<List<CulturalFestival>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.CulturalFestivals.AsNoTracking();
            if (worldId is int w) q = q.Where(f => f.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<CulturalFestival> UpdateAsync(CulturalFestival entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.CulturalFestivals.Where(f => f.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class CulturalInstitutionRepository : ICulturalInstitutionRepository
    {
        private readonly ApplicationDbContext _context;
        public CulturalInstitutionRepository(ApplicationDbContext context) => _context = context;

        public async Task<CulturalInstitution> CreateAsync(CulturalInstitution entity, CancellationToken ct = default)
        {
            _context.CulturalInstitutions.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<CulturalInstitution?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.CulturalInstitutions
                .Include(i => i.History)
                .Include(i => i.Culture)
                .Include(i => i.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, ct);

        public Task<CulturalInstitution?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.CulturalInstitutions.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, ct);

        public async Task<List<CulturalInstitution>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.CulturalInstitutions.AsNoTracking();
            if (worldId is int w) q = q.Where(i => i.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<CulturalInstitution> UpdateAsync(CulturalInstitution entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.CulturalInstitutions.Where(i => i.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> CityExistsInWorldAsync(int cityId, int worldId, CancellationToken ct = default) =>
            _context.Cities.AnyAsync(c => c.Id == cityId && c.WorldId == worldId, ct);
    }
}
