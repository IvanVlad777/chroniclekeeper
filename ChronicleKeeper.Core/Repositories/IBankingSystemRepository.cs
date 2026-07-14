using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IBankingSystemRepository
    {
        Task<BankingSystem> CreateAsync(BankingSystem bankingSystem, CancellationToken cancellationToken = default);
        /// <summary>S uključenim Currency/History — za detail prikaz.</summary>
        Task<BankingSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<BankingSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<BankingSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<BankingSystem> UpdateAsync(BankingSystem bankingSystem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Delete-guard: koliko ekonomskih sustava koristi ovaj bankarski sustav.</summary>
        Task<int> CountEconomicSystemsUsingBankingSystemAsync(int bankingSystemId, CancellationToken cancellationToken = default);
        /// <summary>Delete-guard: koliko korporacija koristi ovaj bankarski sustav.</summary>
        Task<int> CountCorporationsUsingBankingSystemAsync(int bankingSystemId, CancellationToken cancellationToken = default);
    }
}
