using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICultureRepository
    {
        Task<Culture> CreateAsync(Culture culture, CancellationToken cancellationToken = default);
        /// <summary>S uključenim jezikom, religijom i cross-link kolekcijama — za detail prikaz.</summary>
        Task<Culture?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Culture?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Culture>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Culture> UpdateAsync(Culture culture, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsNationLinkedAsync(int cultureId, int nationId, CancellationToken cancellationToken = default);
        Task AddNationAsync(int cultureId, int nationId, CancellationToken cancellationToken = default);
        Task<bool> RemoveNationAsync(int cultureId, int nationId, CancellationToken cancellationToken = default);

        Task<bool> IsSapientSpeciesLinkedAsync(int cultureId, int sapientSpeciesId, CancellationToken cancellationToken = default);
        Task AddSapientSpeciesAsync(int cultureId, int sapientSpeciesId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSapientSpeciesAsync(int cultureId, int sapientSpeciesId, CancellationToken cancellationToken = default);

        Task<bool> IsSocialClassLinkedAsync(int cultureId, int socialClassId, CancellationToken cancellationToken = default);
        Task AddSocialClassAsync(int cultureId, int socialClassId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSocialClassAsync(int cultureId, int socialClassId, CancellationToken cancellationToken = default);
    }
}
