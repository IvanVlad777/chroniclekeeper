using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class HolySiteRepository : IHolySiteRepository
    {
        private readonly ApplicationDbContext _context;
        public HolySiteRepository(ApplicationDbContext context) => _context = context;

        public async Task<HolySite> CreateAsync(HolySite entity, CancellationToken ct = default)
        {
            _context.HolySites.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<HolySite?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.HolySites
                .Include(h => h.History)
                .Include(h => h.Religion)
                .Include(h => h.Deity)
                .Include(h => h.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, ct);

        public Task<HolySite?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.HolySites.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id, ct);

        public async Task<List<HolySite>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.HolySites.AsNoTracking();
            if (worldId is int w) q = q.Where(h => h.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<HolySite> UpdateAsync(HolySite entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.HolySites.Where(h => h.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> IsReferencedByFestivalAsync(int holySiteId, CancellationToken ct = default) =>
            _context.ReligiousFestivals.AnyAsync(f => f.HolySiteId == holySiteId, ct);
    }

    public class ReligiousTextRepository : IReligiousTextRepository
    {
        private readonly ApplicationDbContext _context;
        public ReligiousTextRepository(ApplicationDbContext context) => _context = context;

        public async Task<ReligiousText> CreateAsync(ReligiousText entity, CancellationToken ct = default)
        {
            _context.ReligiousTexts.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<ReligiousText?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.ReligiousTexts
                .Include(t => t.History)
                .Include(t => t.Religion)
                .Include(t => t.Deity)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, ct);

        public Task<ReligiousText?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.ReligiousTexts.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<List<ReligiousText>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.ReligiousTexts.AsNoTracking();
            if (worldId is int w) q = q.Where(t => t.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<ReligiousText> UpdateAsync(ReligiousText entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.ReligiousTexts.Where(t => t.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class ReligiousFestivalRepository : IReligiousFestivalRepository
    {
        private readonly ApplicationDbContext _context;
        public ReligiousFestivalRepository(ApplicationDbContext context) => _context = context;

        public async Task<ReligiousFestival> CreateAsync(ReligiousFestival entity, CancellationToken ct = default)
        {
            _context.ReligiousFestivals.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<ReligiousFestival?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.ReligiousFestivals
                .Include(f => f.History)
                .Include(f => f.Religion)
                .Include(f => f.HolySite)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, ct);

        public Task<ReligiousFestival?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.ReligiousFestivals.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);

        public async Task<List<ReligiousFestival>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.ReligiousFestivals.AsNoTracking();
            if (worldId is int w) q = q.Where(f => f.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<ReligiousFestival> UpdateAsync(ReligiousFestival entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.ReligiousFestivals.Where(f => f.Id == id).ExecuteDeleteAsync(ct) > 0;
    }
}
