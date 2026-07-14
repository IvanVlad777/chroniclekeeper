using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ITaxationSystemRepository
    {
        Task<TaxationSystem> CreateAsync(TaxationSystem taxationSystem, CancellationToken cancellationToken = default);
        /// <summary>S uključenim History-jem — za detail prikaz.</summary>
        Task<TaxationSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TaxationSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TaxationSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<TaxationSystem> UpdateAsync(TaxationSystem taxationSystem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Delete-guard: koliko ekonomskih sustava koristi ovaj porezni sustav.</summary>
        Task<int> CountEconomicSystemsUsingTaxationSystemAsync(int taxationSystemId, CancellationToken cancellationToken = default);
        /// <summary>Delete-guard: koliko cehova koristi ovaj porezni sustav.</summary>
        Task<int> CountGuildsUsingTaxationSystemAsync(int taxationSystemId, CancellationToken cancellationToken = default);
        /// <summary>Delete-guard: koliko korporacija koristi ovaj porezni sustav.</summary>
        Task<int> CountCorporationsUsingTaxationSystemAsync(int taxationSystemId, CancellationToken cancellationToken = default);
    }
}
