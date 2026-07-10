using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class DiplomaticAgreementRepository : IDiplomaticAgreementRepository
    {
        private readonly ApplicationDbContext _context;

        public DiplomaticAgreementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiplomaticAgreement> CreateAsync(DiplomaticAgreement agreement, CancellationToken cancellationToken = default)
        {
            _context.DiplomaticAgreements.Add(agreement);
            await _context.SaveChangesAsync(cancellationToken);
            return agreement;
        }

        public async Task<DiplomaticAgreement?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.DiplomaticAgreements
                .Include(a => a.FirstNation)
                .Include(a => a.SecondNation)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<DiplomaticAgreement?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.DiplomaticAgreements
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<List<DiplomaticAgreement>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.DiplomaticAgreements.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(a => a.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<DiplomaticAgreement> UpdateAsync(DiplomaticAgreement agreement, CancellationToken cancellationToken = default)
        {
            _context.Entry(agreement).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return agreement;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.DiplomaticAgreements
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
