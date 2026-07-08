using ChronicleKeeper.Core.DTOs.Tag;
using ChronicleKeeper.Core.Entities.Tags;

namespace ChronicleKeeper.Core.Repositories
{
    /// <summary>Pokriva tagove i njihovo kačenje na Character/Location/Faction.</summary>
    public interface ITagRepository
    {
        Task<Tag> CreateAsync(Tag tag, CancellationToken cancellationToken = default);
        Task<Tag?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Tag>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Tag> UpdateAsync(Tag tag, CancellationToken cancellationToken = default);
        /// <summary>Briše tag i sve njegove veze (DB cascade).</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Postoji li u svijetu tag s tim imenom (unique per world), opcionalno ignorirajući zadani ID.</summary>
        Task<bool> NameExistsInWorldAsync(string name, int worldId, int? excludeTagId = null, CancellationToken cancellationToken = default);

        // Attach/detach — cilj mora postojati u istom svijetu (provjerava handler)
        Task<bool> TargetExistsInWorldAsync(TagTargetType targetType, int targetId, int worldId, CancellationToken cancellationToken = default);
        Task<bool> IsAttachedAsync(int tagId, TagTargetType targetType, int targetId, CancellationToken cancellationToken = default);
        Task AttachAsync(int tagId, TagTargetType targetType, int targetId, CancellationToken cancellationToken = default);
        Task<bool> DetachAsync(int tagId, TagTargetType targetType, int targetId, CancellationToken cancellationToken = default);
    }
}
