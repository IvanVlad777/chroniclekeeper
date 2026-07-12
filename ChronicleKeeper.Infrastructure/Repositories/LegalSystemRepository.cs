using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class LegalSystemRepository : ILegalSystemRepository
    {
        private readonly ApplicationDbContext _context;

        public LegalSystemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LegalSystem> CreateAsync(LegalSystem legalSystem, CancellationToken cancellationToken = default)
        {
            _context.LegalSystems.Add(legalSystem);
            await _context.SaveChangesAsync(cancellationToken);
            return legalSystem;
        }

        public async Task<LegalSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.LegalSystems
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<LegalSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.LegalSystems
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<List<LegalSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.LegalSystems.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(l => l.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<LegalSystem> UpdateAsync(LegalSystem legalSystem, CancellationToken cancellationToken = default)
        {
            _context.Entry(legalSystem).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return legalSystem;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.LegalSystems
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountLocationsUsingLegalSystemAsync(int legalSystemId, CancellationToken cancellationToken = default)
        {
            var countries = await _context.Countries.CountAsync(c => c.LegalSystemId == legalSystemId, cancellationToken);
            var cities = await _context.Cities.CountAsync(c => c.LegalSystemId == legalSystemId, cancellationToken);
            return countries + cities;
        }
    }
}
