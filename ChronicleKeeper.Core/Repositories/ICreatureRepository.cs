using ChronicleKeeper.Core.Entities.Geography.Creatures;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICreatureRepository
    {
        Task<Creature> CreateAsync(Creature creature, CancellationToken cancellationToken = default);

        /// <summary>Puni graf (ParentCreature, Subspecies, History, Cities) — za detail prikaz.</summary>
        Task<Creature?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Samo korijenski entitet, bez Include-ova.</summary>
        Task<Creature?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Creature>> GetAllAsync(int? worldId = null, string? subtype = null, CancellationToken cancellationToken = default);

        Task<Creature> UpdateAsync(Creature creature, CancellationToken cancellationToken = default);

        /// <summary>Vraća false ako stvorenje ne postoji.</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> HasSubspeciesAsync(int creatureId, CancellationToken cancellationToken = default);

        /// <summary>Bi li postavljanje newParentId kao roditelja stvorenja stvorilo ciklus u hijerarhiji.</summary>
        Task<bool> WouldCreateCycleAsync(int creatureId, int newParentId, CancellationToken cancellationToken = default);

        Task<bool> IsCityLinkedAsync(int creatureId, int cityId, CancellationToken cancellationToken = default);
        Task AddCityAsync(int creatureId, int cityId, CancellationToken cancellationToken = default);
        Task<bool> RemoveCityAsync(int creatureId, int cityId, CancellationToken cancellationToken = default);

        Task<bool> IsHabitatLinkedAsync(int creatureId, int ecosystemId, CancellationToken cancellationToken = default);
        Task AddHabitatAsync(int creatureId, int ecosystemId, CancellationToken cancellationToken = default);
        Task<bool> RemoveHabitatAsync(int creatureId, int ecosystemId, CancellationToken cancellationToken = default);

        // Self-referencing links (symbiosis + predation prey side; predators are read-only reverse).
        Task<bool> IsSymbioticLinkedAsync(int creatureId, int partnerId, CancellationToken cancellationToken = default);
        Task AddSymbiosisAsync(int creatureId, int partnerId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSymbiosisAsync(int creatureId, int partnerId, CancellationToken cancellationToken = default);

        Task<bool> IsPreyLinkedAsync(int predatorId, int preyId, CancellationToken cancellationToken = default);
        Task AddPreyAsync(int predatorId, int preyId, CancellationToken cancellationToken = default);
        Task<bool> RemovePreyAsync(int predatorId, int preyId, CancellationToken cancellationToken = default);
    }
}
