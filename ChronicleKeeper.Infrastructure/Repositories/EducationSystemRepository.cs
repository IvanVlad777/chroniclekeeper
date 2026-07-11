using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class EducationSystemRepository : IEducationSystemRepository
    {
        private readonly ApplicationDbContext _context;

        public EducationSystemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EducationSystem> CreateAsync(EducationSystem educationSystem, CancellationToken cancellationToken = default)
        {
            _context.EducationSystems.Add(educationSystem);
            await _context.SaveChangesAsync(cancellationToken);
            return educationSystem;
        }

        public async Task<EducationSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.EducationSystems
                .Include(e => e.Schools)
                .Include(e => e.Universities)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<EducationSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.EducationSystems
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<EducationSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.EducationSystems.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(e => e.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<EducationSystem> UpdateAsync(EducationSystem educationSystem, CancellationToken cancellationToken = default)
        {
            _context.Entry(educationSystem).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return educationSystem;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Schools i Universities kaskadiraju u DB-u kad se sustav obrazovanja obriše.
            var deleted = await _context.EducationSystems
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
