using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChapterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Chapter> CreateAsync(Chapter chapter, CancellationToken cancellationToken = default)
        {
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync(cancellationToken);
            return chapter;
        }

        public async Task<Chapter?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Chapters
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Chapter>> GetAllAsync(int? worldId = null, int? bookId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Chapters.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            if (bookId is int bid)
            {
                query = query.Where(c => c.BookId == bid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Chapter> UpdateAsync(Chapter chapter, CancellationToken cancellationToken = default)
        {
            _context.Entry(chapter).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return chapter;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Reference.ChapterId is Restrict — clean up first (mirrors ContentRepository.DeleteAsync).
            await _context.References
                .Where(r => r.ChapterId == id)
                .ExecuteDeleteAsync(cancellationToken);

            var deleted = await _context.Chapters
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
