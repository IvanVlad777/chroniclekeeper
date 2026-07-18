using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ApplicationDbContext _context;

        public LibraryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Library> CreateAsync(Library library, CancellationToken cancellationToken = default)
        {
            _context.Libraries.Add(library);
            await _context.SaveChangesAsync(cancellationToken);
            return library;
        }

        public async Task<Library?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Libraries
                .Include(l => l.University)
                .Include(l => l.Location)
                .Include(l => l.Scholars).ThenInclude(ls => ls.Character)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<Library?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Libraries
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<List<Library>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Libraries
                .Include(l => l.University)
                .Include(l => l.Location)
                .AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(l => l.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Library> UpdateAsync(Library library, CancellationToken cancellationToken = default)
        {
            _context.Entry(library).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return library;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Libraries
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public Task<bool> IsScholarLinkedAsync(int libraryId, int characterId, CancellationToken cancellationToken = default)
            => _context.LibraryScholars.AnyAsync(x => x.LibraryId == libraryId && x.CharacterId == characterId, cancellationToken);

        public async Task AddScholarAsync(int libraryId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.LibraryScholars.Add(new LibraryScholar { LibraryId = libraryId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveScholarAsync(int libraryId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.LibraryScholars
                .Where(x => x.LibraryId == libraryId && x.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
