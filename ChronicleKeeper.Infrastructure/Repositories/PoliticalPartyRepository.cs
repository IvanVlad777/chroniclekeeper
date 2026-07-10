using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class PoliticalPartyRepository : IPoliticalPartyRepository
    {
        private readonly ApplicationDbContext _context;

        public PoliticalPartyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PoliticalParty> CreateAsync(PoliticalParty party, CancellationToken cancellationToken = default)
        {
            _context.PoliticalParties.Add(party);
            await _context.SaveChangesAsync(cancellationToken);
            return party;
        }

        public async Task<PoliticalParty?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalParties
                .Include(p => p.PoliticalIdeology)
                .Include(p => p.GovernmentSystem)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PoliticalParty?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalParties
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<PoliticalParty>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.PoliticalParties.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(p => p.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<PoliticalParty> UpdateAsync(PoliticalParty party, CancellationToken cancellationToken = default)
        {
            _context.Entry(party).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return party;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.PoliticalParties
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
