using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ILanguageRepository
    {
        Task<Language> CreateAsync(Language language, CancellationToken cancellationToken = default);
        /// <summary>S uključenim kulturama — za detail prikaz.</summary>
        Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Language?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Language>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Language> UpdateAsync(Language language, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountCulturesUsingLanguageAsync(int languageId, CancellationToken cancellationToken = default);

        Task<bool> IsNationLinkedAsync(int languageId, int nationId, CancellationToken cancellationToken = default);
        Task AddNationAsync(int languageId, int nationId, CancellationToken cancellationToken = default);
        Task<bool> RemoveNationAsync(int languageId, int nationId, CancellationToken cancellationToken = default);
    }
}
