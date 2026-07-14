using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICorporationRepository
    {
        Task<Corporation> CreateAsync(Corporation corporation, CancellationToken cancellationToken = default);
        /// <summary>S uključenim sustavima, parentom, History-jem, vodstvom i cross-link kolekcijama — za detail prikaz.</summary>
        Task<Corporation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Corporation?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Corporation>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Corporation> UpdateAsync(Corporation corporation, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Delete-guard: ima li korporacija podružnice.</summary>
        Task<bool> HasSubsidiariesAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Cycle-guard: bi li postavljanje parenta stvorilo ciklus u hijerarhiji podružnica.</summary>
        Task<bool> WouldCreateCycleAsync(int corporationId, int newParentId, CancellationToken cancellationToken = default);

        Task<bool> IsFactionLinkedAsync(int corporationId, int factionId, CancellationToken cancellationToken = default);
        Task AddFactionAsync(int corporationId, int factionId, CancellationToken cancellationToken = default);
        Task<bool> RemoveFactionAsync(int corporationId, int factionId, CancellationToken cancellationToken = default);

        Task<bool> IsProfessionLinkedAsync(int corporationId, int professionId, CancellationToken cancellationToken = default);
        Task AddProfessionAsync(int corporationId, int professionId, CancellationToken cancellationToken = default);
        Task<bool> RemoveProfessionAsync(int corporationId, int professionId, CancellationToken cancellationToken = default);
    }
}
