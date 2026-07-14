using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken = default);
        /// <summary>S uključenim History-jem — za detail prikaz.</summary>
        Task<Currency?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Currency?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Currency>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Currency> UpdateAsync(Currency currency, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Delete-guard: koliko bankarskih sustava koristi ovu valutu.</summary>
        Task<int> CountBankingSystemsUsingCurrencyAsync(int currencyId, CancellationToken cancellationToken = default);
    }
}
