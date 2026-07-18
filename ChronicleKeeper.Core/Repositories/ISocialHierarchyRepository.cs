using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ISocialHierarchyRepository
    {
        Task<SocialHierarchy> CreateAsync(SocialHierarchy hierarchy, CancellationToken cancellationToken = default);
        /// <summary>With member classes, nations and history — for detail view.</summary>
        Task<SocialHierarchy?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<SocialHierarchy?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<SocialHierarchy>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<SocialHierarchy> UpdateAsync(SocialHierarchy hierarchy, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
