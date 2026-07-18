using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class PrivilegeLawRepository : IPrivilegeLawRepository
    {
        private readonly ApplicationDbContext _context;

        public PrivilegeLawRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PrivilegeLaw> CreateAsync(PrivilegeLaw law, CancellationToken cancellationToken = default)
        {
            _context.PrivilegeLaws.Add(law);
            await _context.SaveChangesAsync(cancellationToken);
            return law;
        }

        public async Task<PrivilegeLaw?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.PrivilegeLaws
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<PrivilegeLaw>> GetAllAsync(int? worldId = null, int? socialClassId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.PrivilegeLaws.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(p => p.WorldId == wid);
            }
            if (socialClassId is int scid)
            {
                query = query.Where(p => p.SocialClassId == scid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<PrivilegeLaw> UpdateAsync(PrivilegeLaw law, CancellationToken cancellationToken = default)
        {
            _context.Entry(law).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return law;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.PrivilegeLaws
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
