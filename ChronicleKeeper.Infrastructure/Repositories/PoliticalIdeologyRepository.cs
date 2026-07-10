using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class PoliticalIdeologyRepository : IPoliticalIdeologyRepository
    {
        private readonly ApplicationDbContext _context;

        public PoliticalIdeologyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PoliticalIdeology> CreateAsync(PoliticalIdeology ideology, CancellationToken cancellationToken = default)
        {
            _context.PoliticalIdeologies.Add(ideology);
            await _context.SaveChangesAsync(cancellationToken);
            return ideology;
        }

        public async Task<PoliticalIdeology?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalIdeologies
                .Include(i => i.AffiliatedPoliticalParties)
                .Include(i => i.AffiliatedGovernmentSystems)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<PoliticalIdeology?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalIdeologies
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<List<PoliticalIdeology>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.PoliticalIdeologies.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(i => i.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<PoliticalIdeology> UpdateAsync(PoliticalIdeology ideology, CancellationToken cancellationToken = default)
        {
            _context.Entry(ideology).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return ideology;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.PoliticalIdeologies
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountPoliticalPartiesUsingIdeologyAsync(int ideologyId, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalParties.CountAsync(p => p.PoliticalIdeologyId == ideologyId, cancellationToken);
        }

        public async Task<int> CountGovernmentSystemsUsingIdeologyAsync(int ideologyId, CancellationToken cancellationToken = default)
        {
            return await _context.GovernmentSystems.CountAsync(g => g.PoliticalIdeologyId == ideologyId, cancellationToken);
        }
    }
}
