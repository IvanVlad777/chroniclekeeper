using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class HobbyRepository : IHobbyRepository
    {
        private readonly ApplicationDbContext _context;

        public HobbyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Hobby> CreateAsync(Hobby hobby, CancellationToken cancellationToken = default)
        {
            _context.Hobbies.Add(hobby);
            await _context.SaveChangesAsync(cancellationToken);
            return hobby;
        }

        public async Task<Hobby?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Hobbies
                .Include(h => h.History)
                .Include(h => h.Practitioners).ThenInclude(x => x.Character)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<Hobby?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Hobbies
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<Hobby>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Hobbies.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(h => h.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Hobby> UpdateAsync(Hobby hobby, CancellationToken cancellationToken = default)
        {
            _context.Entry(hobby).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return hobby;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Hobbies
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
