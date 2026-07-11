using ChronicleKeeper.Core.Entities.Characters.Equipment;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IItemRepository
    {
        Task<Item> CreateAsync(Item item, CancellationToken cancellationToken = default);
        /// <summary>Puni graf (trenutni vlasnik, lokacija, frakcija, povijest vlasništva) — za detail prikaz.</summary>
        Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Samo korijenski entitet, bez Include-ova — za update/interne provjere.</summary>
        Task<Item?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Item>> GetAllAsync(int? worldId = null, int? currentOwnerId = null, int? factionId = null, CancellationToken cancellationToken = default);
        Task<Item> UpdateAsync(Item item, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
