using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ReligionRepository : IReligionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReligionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Religion> CreateAsync(Religion religion, CancellationToken cancellationToken = default)
        {
            _context.Religions.Add(religion);
            await _context.SaveChangesAsync(cancellationToken);
            return religion;
        }

        public async Task<Religion?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Religions
                .Include(r => r.Followers)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<Religion?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Religions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<Religion>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Religions.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(r => r.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Religion> UpdateAsync(Religion religion, CancellationToken cancellationToken = default)
        {
            _context.Entry(religion).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return religion;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Religions
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountCharactersUsingReligionAsync(int religionId, CancellationToken cancellationToken = default)
        {
            return await _context.Characters.CountAsync(c => c.ReligionId == religionId, cancellationToken);
        }

        public async Task<int> CountCulturesUsingReligionAsync(int religionId, CancellationToken cancellationToken = default)
        {
            return await _context.Cultures.CountAsync(c => c.ReligionId == religionId, cancellationToken);
        }
    }
}
