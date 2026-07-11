using ChronicleKeeper.Core.Entities.Content.Movie;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly ApplicationDbContext _context;

        public EpisodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Episode> CreateAsync(Episode episode, CancellationToken cancellationToken = default)
        {
            _context.Episodes.Add(episode);
            await _context.SaveChangesAsync(cancellationToken);
            return episode;
        }

        public async Task<Episode?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Episodes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<Episode>> GetAllAsync(int? worldId = null, int? seriesId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Episodes.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(e => e.WorldId == wid);
            }
            if (seriesId is int sid)
            {
                query = query.Where(e => e.SeriesId == sid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Episode> UpdateAsync(Episode episode, CancellationToken cancellationToken = default)
        {
            _context.Entry(episode).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return episode;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Reference.EpisodeId is Restrict — clean up first (mirrors ContentRepository.DeleteAsync).
            await _context.References
                .Where(r => r.EpisodeId == id)
                .ExecuteDeleteAsync(cancellationToken);

            var deleted = await _context.Episodes
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
