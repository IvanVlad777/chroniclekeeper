using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IExtractionMethodRepository
    {
        Task<ExtractionMethod> CreateAsync(ExtractionMethod extractionMethod, CancellationToken cancellationToken = default);
        /// <summary>S uključenim History-jem i resursima — za detail prikaz.</summary>
        Task<ExtractionMethod?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ExtractionMethod?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ExtractionMethod>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<ExtractionMethod> UpdateAsync(ExtractionMethod extractionMethod, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Delete-guard: koliko prirodnih resursa koristi ovu metodu ekstrakcije.</summary>
        Task<int> CountNaturalResourcesUsingExtractionMethodAsync(int extractionMethodId, CancellationToken cancellationToken = default);
    }
}
