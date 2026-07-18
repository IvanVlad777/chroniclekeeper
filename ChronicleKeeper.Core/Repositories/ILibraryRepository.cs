using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ILibraryRepository
    {
        Task<Library> CreateAsync(Library library, CancellationToken cancellationToken = default);
        /// <summary>S uključenim University/Location referencama — koristi se i za listu i za pojedinačni prikaz.</summary>
        Task<Library?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Samo korijenski entitet, bez Include-ova — za update/interne provjere.</summary>
        Task<Library?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Library>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Library> UpdateAsync(Library library, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        // Scholars (LibraryScholar join with Character)
        Task<bool> IsScholarLinkedAsync(int libraryId, int characterId, CancellationToken cancellationToken = default);
        Task AddScholarAsync(int libraryId, int characterId, CancellationToken cancellationToken = default);
        Task<bool> RemoveScholarAsync(int libraryId, int characterId, CancellationToken cancellationToken = default);
    }
}
