using ChronicleKeeper.Core.Entities.Notes;

namespace ChronicleKeeper.Core.Repositories
{
    public interface INoteRepository
    {
        Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default);
        Task<Note?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Note>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
