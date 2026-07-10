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
                .Include(p => p.Factions).ThenInclude(pf => pf.Faction)
                .Include(p => p.Nations).ThenInclude(pn => pn.Nation)
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

        public async Task<bool> IsFactionLinkedAsync(int politicalPartyId, int factionId, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalPartyFactions
                .AnyAsync(pf => pf.PoliticalPartyId == politicalPartyId && pf.FactionId == factionId, cancellationToken);
        }

        public async Task AddFactionAsync(int politicalPartyId, int factionId, CancellationToken cancellationToken = default)
        {
            _context.PoliticalPartyFactions.Add(new PoliticalPartyFaction { PoliticalPartyId = politicalPartyId, FactionId = factionId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveFactionAsync(int politicalPartyId, int factionId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.PoliticalPartyFactions
                .Where(pf => pf.PoliticalPartyId == politicalPartyId && pf.FactionId == factionId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsNationLinkedAsync(int politicalPartyId, int nationId, CancellationToken cancellationToken = default)
        {
            return await _context.PoliticalPartyNations
                .AnyAsync(pn => pn.PoliticalPartyId == politicalPartyId && pn.NationId == nationId, cancellationToken);
        }

        public async Task AddNationAsync(int politicalPartyId, int nationId, CancellationToken cancellationToken = default)
        {
            _context.PoliticalPartyNations.Add(new PoliticalPartyNation { PoliticalPartyId = politicalPartyId, NationId = nationId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveNationAsync(int politicalPartyId, int nationId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.PoliticalPartyNations
                .Where(pn => pn.PoliticalPartyId == politicalPartyId && pn.NationId == nationId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
