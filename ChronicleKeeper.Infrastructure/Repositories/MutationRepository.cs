using ChronicleKeeper.Core.Entities.Miscellaneous;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class MutationRepository : IMutationRepository
    {
        private readonly ApplicationDbContext _context;

        public MutationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Mutation> CreateAsync(Mutation mutation, CancellationToken cancellationToken = default)
        {
            _context.Mutations.Add(mutation);
            await _context.SaveChangesAsync(cancellationToken);
            return mutation;
        }

        public async Task<Mutation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Mutations
                .Include(m => m.MutantCreature)
                .Include(m => m.History)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<Mutation?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Mutations
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<List<Mutation>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Mutations.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(m => m.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Mutation> UpdateAsync(Mutation mutation, CancellationToken cancellationToken = default)
        {
            _context.Entry(mutation).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return mutation;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Mutations
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
