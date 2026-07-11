using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ApprenticeshipRepository : IApprenticeshipRepository
    {
        private readonly ApplicationDbContext _context;

        public ApprenticeshipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Apprenticeship> CreateAsync(Apprenticeship apprenticeship, CancellationToken cancellationToken = default)
        {
            _context.Apprenticeships.Add(apprenticeship);
            await _context.SaveChangesAsync(cancellationToken);
            return apprenticeship;
        }

        public async Task<Apprenticeship?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Apprenticeships
                .Include(a => a.TradeSchool)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Apprenticeship?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Apprenticeships
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<List<Apprenticeship>> GetAllAsync(int? worldId = null, int? professionId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Apprenticeships.Include(a => a.TradeSchool).AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(a => a.WorldId == wid);
            }
            if (professionId is int pid)
            {
                query = query.Where(a => a.ProfessionId == pid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Apprenticeship> UpdateAsync(Apprenticeship apprenticeship, CancellationToken cancellationToken = default)
        {
            _context.Entry(apprenticeship).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return apprenticeship;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Apprenticeships
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
