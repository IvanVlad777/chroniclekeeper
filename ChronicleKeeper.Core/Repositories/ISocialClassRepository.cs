using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ISocialClassRepository
    {
        Task<SocialClass> CreateAsync(SocialClass socialClass, CancellationToken cancellationToken = default);
        /// <summary>S uključenim članovima — za detail prikaz.</summary>
        Task<SocialClass?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<SocialClass?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<SocialClass>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<SocialClass> UpdateAsync(SocialClass socialClass, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountCharactersUsingSocialClassAsync(int socialClassId, CancellationToken cancellationToken = default);
    }
}
