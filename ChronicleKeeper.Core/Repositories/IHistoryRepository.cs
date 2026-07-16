using ChronicleKeeper.Core.DTOs.History;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IHistoryRepository
    {
        Task<History> CreateAsync(History history, CancellationToken cancellationToken = default);
        /// <summary>Puni graf (Timelines + njihovi Events) — za detail prikaz.</summary>
        Task<History?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Entiteti (Character/Location/Faction/Nation) koji pokazuju na ovu povijest.</summary>
        Task<List<HistoryLinkDto>> GetLinkedEntitiesAsync(int historyId, CancellationToken cancellationToken = default);
        /// <summary>Samo korijenski entitet, bez Include-ova — za update/interne provjere.</summary>
        Task<History?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<History>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<History> UpdateAsync(History history, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
