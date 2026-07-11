using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class SpecialisationRepository : ISpecialisationRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecialisationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Specialisation> CreateAsync(Specialisation specialisation, CancellationToken cancellationToken = default)
        {
            _context.Specialisations.Add(specialisation);
            await _context.SaveChangesAsync(cancellationToken);
            return specialisation;
        }

        public async Task<Specialisation?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Specialisations
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Specialisation>> GetAllAsync(int? worldId = null, int? professionId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Specialisations.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(s => s.WorldId == wid);
            }
            if (professionId is int pid)
            {
                query = query.Where(s => s.ProfessionId == pid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Specialisation> UpdateAsync(Specialisation specialisation, CancellationToken cancellationToken = default)
        {
            _context.Entry(specialisation).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return specialisation;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Specialisations
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
