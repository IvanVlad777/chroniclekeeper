using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class GovernmentSystemRepository : IGovernmentSystemRepository
    {
        private readonly ApplicationDbContext _context;

        public GovernmentSystemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GovernmentSystem> CreateAsync(GovernmentSystem governmentSystem, CancellationToken cancellationToken = default)
        {
            _context.GovernmentSystems.Add(governmentSystem);
            await _context.SaveChangesAsync(cancellationToken);
            return governmentSystem;
        }

        public async Task<GovernmentSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.GovernmentSystems
                .Include(g => g.PoliticalIdeology)
                .Include(g => g.PoliticalParties)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<GovernmentSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.GovernmentSystems
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<List<GovernmentSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.GovernmentSystems.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(g => g.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<GovernmentSystem> UpdateAsync(GovernmentSystem governmentSystem, CancellationToken cancellationToken = default)
        {
            _context.Entry(governmentSystem).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return governmentSystem;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.GovernmentSystems
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountPoliticalPartiesUsingGovernmentSystemAsync(int governmentSystemId, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalParties.CountAsync(p => p.GovernmentSystemId == governmentSystemId, cancellationToken);
        }
    }
}
