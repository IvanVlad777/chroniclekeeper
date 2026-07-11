using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ReferenceRepository : IReferenceRepository
    {
        private readonly ApplicationDbContext _context;

        public ReferenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reference> CreateAsync(Reference reference, CancellationToken cancellationToken = default)
        {
            _context.References.Add(reference);
            await _context.SaveChangesAsync(cancellationToken);
            return reference;
        }

        public async Task<Reference?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.References
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<Reference>> GetAllAsync(
            int? contentId = null,
            int? chapterId = null,
            int? episodeId = null,
            int? characterId = null,
            int? locationId = null,
            int? factionId = null,
            int? nationId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.References.AsNoTracking();
            if (contentId is int cid) query = query.Where(r => r.ContentId == cid);
            if (chapterId is int chId) query = query.Where(r => r.ChapterId == chId);
            if (episodeId is int epId) query = query.Where(r => r.EpisodeId == epId);
            if (characterId is int charId) query = query.Where(r => r.CharacterId == charId);
            if (locationId is int locId) query = query.Where(r => r.LocationId == locId);
            if (factionId is int facId) query = query.Where(r => r.FactionId == facId);
            if (nationId is int natId) query = query.Where(r => r.NationId == natId);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Reference> UpdateAsync(Reference reference, CancellationToken cancellationToken = default)
        {
            _context.Entry(reference).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return reference;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Leaf entity — nothing else points at a Reference, no pre-cleanup needed.
            var deleted = await _context.References
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
