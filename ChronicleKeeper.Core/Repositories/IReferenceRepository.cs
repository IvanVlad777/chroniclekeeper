using ChronicleKeeper.Core.Entities.Content;

namespace ChronicleKeeper.Core.Repositories
{
    /// <summary>Repository for the <c>Reference</c> join-style entity. Leaf entity — no delete guard needed.</summary>
    public interface IReferenceRepository
    {
        Task<Reference> CreateAsync(Reference reference, CancellationToken cancellationToken = default);
        Task<Reference?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>All filters are optional and combined with AND — callers typically set exactly one
        /// (e.g. ?chapterId=5 to list everything referenced in Chapter 5, or ?characterId=12 to list
        /// everything Character 12 appears in), but nothing here restricts the combination.</summary>
        Task<List<Reference>> GetAllAsync(
            int? contentId = null,
            int? chapterId = null,
            int? episodeId = null,
            int? characterId = null,
            int? locationId = null,
            int? factionId = null,
            int? nationId = null,
            CancellationToken cancellationToken = default);

        Task<Reference> UpdateAsync(Reference reference, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
