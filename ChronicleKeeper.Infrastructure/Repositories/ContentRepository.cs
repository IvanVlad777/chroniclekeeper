using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Entities.Content.Movie;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly ApplicationDbContext _context;

        public ContentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Content> CreateAsync(Content content, CancellationToken cancellationToken = default)
        {
            _context.Contents.Add(content);
            await _context.SaveChangesAsync(cancellationToken);
            return content;
        }

        public async Task<Content?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Contents
                .Include(c => c.References).ThenInclude(r => r.Character)
                .Include(c => c.References).ThenInclude(r => r.Location)
                .Include(c => c.References).ThenInclude(r => r.Faction)
                .Include(c => c.References).ThenInclude(r => r.Nation)
                .Include(c => ((Book)c).Chapters)
                .Include(c => ((Series)c).Episodes)
                .Include(c => ((Movie)c).Prequel)
                .Include(c => ((Movie)c).Sequels)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Content?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Contents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Content>> GetAllAsync(int? worldId = null, string? type = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Contents.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                // "ContentType" is the shadow discriminator property configured in ContentConfiguration.
                query = query.Where(c => EF.Property<string>(c, "ContentType") == type);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Content> UpdateAsync(Content content, CancellationToken cancellationToken = default)
        {
            _context.Entry(content).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return content;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Reference.ContentId/ChapterId/EpisodeId are Restrict (see ReferenceConfiguration) — a
            // direct Contents->References Cascade combined with Contents->Chapters->References and
            // Contents->Episodes->References would create SQL Server "multiple cascade paths". Manually
            // clean up any References pointing at this Content directly, or at its Chapters/Episodes
            // (which cascade-delete automatically once the Content row itself is removed).
            await _context.References
                .Where(r => r.ContentId == id)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.References
                .Where(r => r.ChapterId != null && r.Chapter!.BookId == id)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.References
                .Where(r => r.EpisodeId != null && r.Episode!.SeriesId == id)
                .ExecuteDeleteAsync(cancellationToken);

            var deleted = await _context.Contents
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
