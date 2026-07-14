using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IIndustryRepository
    {
        Task<Industry> CreateAsync(Industry industry, CancellationToken cancellationToken = default);
        /// <summary>S uključenim History-jem — za detail prikaz.</summary>
        Task<Industry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Industry?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Industry>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Industry> UpdateAsync(Industry industry, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Delete-guard: koliko cehova pripada ovoj industriji.</summary>
        Task<int> CountGuildsUsingIndustryAsync(int industryId, CancellationToken cancellationToken = default);
        /// <summary>Delete-guard: koliko korporacija pripada ovoj industriji.</summary>
        Task<int> CountCorporationsUsingIndustryAsync(int industryId, CancellationToken cancellationToken = default);
    }
}
