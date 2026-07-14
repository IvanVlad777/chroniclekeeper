using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IEconomicSystemRepository
    {
        Task<EconomicSystem> CreateAsync(EconomicSystem economicSystem, CancellationToken cancellationToken = default);
        /// <summary>S uključenim TaxationSystem/BankingSystem/History — za detail prikaz.</summary>
        Task<EconomicSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<EconomicSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<EconomicSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<EconomicSystem> UpdateAsync(EconomicSystem economicSystem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Delete-guard: koliko lokacija (Country/City) koristi ovaj ekonomski sustav.</summary>
        Task<int> CountLocationsUsingEconomicSystemAsync(int economicSystemId, CancellationToken cancellationToken = default);
    }
}
