using ChronicleKeeper.Core.Entities.Content.Book;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IChapterRepository
    {
        Task<Chapter> CreateAsync(Chapter chapter, CancellationToken cancellationToken = default);
        Task<Chapter?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Chapter>> GetAllAsync(int? worldId = null, int? bookId = null, CancellationToken cancellationToken = default);
        Task<Chapter> UpdateAsync(Chapter chapter, CancellationToken cancellationToken = default);

        /// <summary>Vraća false ako poglavlje ne postoji. Prije brisanja čisti References koje ciljaju
        /// ovo poglavlje (Reference.ChapterId je Restrict).</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
