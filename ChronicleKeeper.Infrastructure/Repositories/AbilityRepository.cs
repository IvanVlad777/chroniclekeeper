using ChronicleKeeper.Core.Entities.Characters.Abilities;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class AbilityRepository : IAbilityRepository
    {
        private readonly ApplicationDbContext _context;

        public AbilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ability> CreateAsync(Ability ability, CancellationToken cancellationToken = default)
        {
            _context.Abilities.Add(ability);
            await _context.SaveChangesAsync(cancellationToken);
            return ability;
        }

        public async Task<Ability?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Abilities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<List<Ability>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Abilities.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(a => a.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Ability> UpdateAsync(Ability ability, CancellationToken cancellationToken = default)
        {
            _context.Entry(ability).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return ability;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Abilities
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
