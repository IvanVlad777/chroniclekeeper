using ChronicleKeeper.Core.Entities.Content;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IContentRepository
    {
        Task<Content> CreateAsync(Content content, CancellationToken cancellationToken = default);

        /// <summary>Puni graf (References + entity side, Chapters/Episodes/Prequel/Sequels ovisno o tipu) — za detail prikaz.</summary>
        Task<Content?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Samo korijenski entitet, bez Include-ova — za update/delete/cross-vertical provjere (npr. Chapter/Episode validirajući svog roditelja).</summary>
        Task<Content?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Content>> GetAllAsync(int? worldId = null, string? type = null, CancellationToken cancellationToken = default);

        Task<Content> UpdateAsync(Content content, CancellationToken cancellationToken = default);

        /// <summary>Vraća false ako sadržaj ne postoji. Prije brisanja čisti References koje bi
        /// inače blokirale kaskadu (Reference.ContentId/ChapterId/EpisodeId su Restrict).</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
