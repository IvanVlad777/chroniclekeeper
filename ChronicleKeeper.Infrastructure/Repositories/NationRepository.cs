using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class NationRepository : INationRepository
    {
        private readonly ApplicationDbContext _context;

        public NationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Nation> CreateAsync(Nation nation, CancellationToken cancellationToken = default)
        {
            _context.Nations.Add(nation);
            await _context.SaveChangesAsync(cancellationToken);
            return nation;
        }

        public async Task<Nation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Nations
                .Include(n => n.Characters)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task<Nation?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Nations
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task<List<Nation>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Nations.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(n => n.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Nation> UpdateAsync(Nation nation, CancellationToken cancellationToken = default)
        {
            _context.Entry(nation).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return nation;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Nations
                .Where(n => n.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountCharactersUsingNationAsync(int nationId, CancellationToken cancellationToken = default)
        {
            return await _context.Characters.CountAsync(c => c.NationId == nationId, cancellationToken);
        }

        public async Task<int> CountDiplomaticAgreementsUsingNationAsync(int nationId, CancellationToken cancellationToken = default)
        {
            return await _context.DiplomaticAgreements
                .CountAsync(a => a.FirstNationId == nationId || a.SecondNationId == nationId, cancellationToken);
        }
    }
}
