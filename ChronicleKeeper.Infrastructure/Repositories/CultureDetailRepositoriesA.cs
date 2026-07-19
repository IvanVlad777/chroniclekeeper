using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class CustomRepository : ICustomRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomRepository(ApplicationDbContext context) => _context = context;

        public async Task<Custom> CreateAsync(Custom entity, CancellationToken ct = default)
        {
            _context.Customs.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Custom?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Customs
                .Include(c => c.History)
                .Include(c => c.Culture)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public Task<Custom?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Customs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<List<Custom>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Customs.AsNoTracking();
            if (worldId is int w) q = q.Where(c => c.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Custom> UpdateAsync(Custom entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Customs.Where(c => c.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class ArtFormRepository : IArtFormRepository
    {
        private readonly ApplicationDbContext _context;
        public ArtFormRepository(ApplicationDbContext context) => _context = context;

        public async Task<ArtForm> CreateAsync(ArtForm entity, CancellationToken ct = default)
        {
            _context.ArtForms.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<ArtForm?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.ArtForms
                .Include(a => a.History)
                .Include(a => a.Culture)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, ct);

        public Task<ArtForm?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.ArtForms.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

        public async Task<List<ArtForm>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.ArtForms.AsNoTracking();
            if (worldId is int w) q = q.Where(a => a.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<ArtForm> UpdateAsync(ArtForm entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.ArtForms.Where(a => a.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class CuisineRepository : ICuisineRepository
    {
        private readonly ApplicationDbContext _context;
        public CuisineRepository(ApplicationDbContext context) => _context = context;

        public async Task<Cuisine> CreateAsync(Cuisine entity, CancellationToken ct = default)
        {
            _context.Cuisines.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Cuisine?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Cuisines
                .Include(c => c.History)
                .Include(c => c.Culture)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public Task<Cuisine?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Cuisines.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<List<Cuisine>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Cuisines.AsNoTracking();
            if (worldId is int w) q = q.Where(c => c.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Cuisine> UpdateAsync(Cuisine entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Cuisines.Where(c => c.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class ClothingRepository : IClothingRepository
    {
        private readonly ApplicationDbContext _context;
        public ClothingRepository(ApplicationDbContext context) => _context = context;

        public async Task<Clothing> CreateAsync(Clothing entity, CancellationToken ct = default)
        {
            _context.Clothing.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Clothing?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Clothing
                .Include(c => c.History)
                .Include(c => c.Culture)
                .Include(c => c.Wearers).ThenInclude(x => x.Character)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public Task<Clothing?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Clothing.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<List<Clothing>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Clothing.AsNoTracking();
            if (worldId is int w) q = q.Where(c => c.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Clothing> UpdateAsync(Clothing entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Clothing.Where(c => c.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class TraditionRepository : ITraditionRepository
    {
        private readonly ApplicationDbContext _context;
        public TraditionRepository(ApplicationDbContext context) => _context = context;

        public async Task<Tradition> CreateAsync(Tradition entity, CancellationToken ct = default)
        {
            _context.Traditions.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Tradition?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Traditions
                .Include(t => t.History)
                .Include(t => t.Culture)
                .Include(t => t.Religion)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, ct);

        public Task<Tradition?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Traditions.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<List<Tradition>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Traditions.AsNoTracking();
            if (worldId is int w) q = q.Where(t => t.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Tradition> UpdateAsync(Tradition entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Traditions.Where(t => t.Id == id).ExecuteDeleteAsync(ct) > 0;
    }
}
