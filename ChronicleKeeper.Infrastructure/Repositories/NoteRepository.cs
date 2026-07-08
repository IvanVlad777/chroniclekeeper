using ChronicleKeeper.Core.Entities.Notes;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync(cancellationToken);
            return note;
        }

        public async Task<Note?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Notes
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task<List<Note>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Notes.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(n => n.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default)
        {
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return note;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Notes
                .Where(n => n.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
