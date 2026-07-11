using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class JobRankRepository : IJobRankRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRankRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobRank> CreateAsync(JobRank jobRank, CancellationToken cancellationToken = default)
        {
            _context.JobRanks.Add(jobRank);
            await _context.SaveChangesAsync(cancellationToken);
            return jobRank;
        }

        public async Task<JobRank?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.JobRanks
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        }

        public async Task<List<JobRank>> GetAllAsync(int? worldId = null, int? professionId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.JobRanks.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(j => j.WorldId == wid);
            }
            if (professionId is int pid)
            {
                query = query.Where(j => j.ProfessionId == pid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<JobRank> UpdateAsync(JobRank jobRank, CancellationToken cancellationToken = default)
        {
            _context.Entry(jobRank).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return jobRank;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.JobRanks
                .Where(j => j.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
