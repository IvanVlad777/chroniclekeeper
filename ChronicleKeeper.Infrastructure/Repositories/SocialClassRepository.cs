using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class SocialClassRepository : ISocialClassRepository
    {
        private readonly ApplicationDbContext _context;

        public SocialClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SocialClass> CreateAsync(SocialClass socialClass, CancellationToken cancellationToken = default)
        {
            _context.SocialClasses.Add(socialClass);
            await _context.SaveChangesAsync(cancellationToken);
            return socialClass;
        }

        public async Task<SocialClass?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SocialClasses
                .Include(sc => sc.Members)
                .AsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken);
        }

        public async Task<SocialClass?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SocialClasses
                .AsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken);
        }

        public async Task<List<SocialClass>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.SocialClasses.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(sc => sc.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<SocialClass> UpdateAsync(SocialClass socialClass, CancellationToken cancellationToken = default)
        {
            _context.Entry(socialClass).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return socialClass;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.SocialClasses
                .Where(sc => sc.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountCharactersUsingSocialClassAsync(int socialClassId, CancellationToken cancellationToken = default)
        {
            return await _context.Characters.CountAsync(c => c.SocialClassId == socialClassId, cancellationToken);
        }
    }
}
