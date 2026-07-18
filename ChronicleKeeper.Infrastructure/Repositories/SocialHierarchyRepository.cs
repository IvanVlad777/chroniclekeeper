using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class SocialHierarchyRepository : ISocialHierarchyRepository
    {
        private readonly ApplicationDbContext _context;

        public SocialHierarchyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SocialHierarchy> CreateAsync(SocialHierarchy hierarchy, CancellationToken cancellationToken = default)
        {
            _context.SocialHierarchies.Add(hierarchy);
            await _context.SaveChangesAsync(cancellationToken);
            return hierarchy;
        }

        public async Task<SocialHierarchy?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SocialHierarchies
                .Include(h => h.History)
                .Include(h => h.Classes)
                .Include(h => h.Nations)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<SocialHierarchy?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SocialHierarchies
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<SocialHierarchy>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.SocialHierarchies.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(h => h.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<SocialHierarchy> UpdateAsync(SocialHierarchy hierarchy, CancellationToken cancellationToken = default)
        {
            _context.Entry(hierarchy).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return hierarchy;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.SocialHierarchies
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
