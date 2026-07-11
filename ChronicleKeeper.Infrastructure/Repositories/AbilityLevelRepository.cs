using ChronicleKeeper.Core.Entities.Characters.Abilities;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class AbilityLevelRepository : IAbilityLevelRepository
    {
        private readonly ApplicationDbContext _context;

        public AbilityLevelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AbilityLevel> CreateAsync(AbilityLevel abilityLevel, CancellationToken cancellationToken = default)
        {
            _context.AbilityLevels.Add(abilityLevel);
            await _context.SaveChangesAsync(cancellationToken);
            return abilityLevel;
        }

        public async Task<AbilityLevel?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.AbilityLevels
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<List<AbilityLevel>> GetAllAsync(int? worldId = null, int? abilityId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.AbilityLevels.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(l => l.WorldId == wid);
            }
            if (abilityId is int aid)
            {
                query = query.Where(l => l.AbilityId == aid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<AbilityLevel> UpdateAsync(AbilityLevel abilityLevel, CancellationToken cancellationToken = default)
        {
            _context.Entry(abilityLevel).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return abilityLevel;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.AbilityLevels
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
