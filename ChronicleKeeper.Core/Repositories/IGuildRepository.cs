using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IGuildRepository
    {
        Task<Guild> CreateAsync(Guild guild, CancellationToken cancellationToken = default);
        /// <summary>S uključenim sustavima, History-jem, rangovima i cross-link kolekcijama — za detail prikaz.</summary>
        Task<Guild?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Guild?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Guild>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Guild> UpdateAsync(Guild guild, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsFactionLinkedAsync(int guildId, int factionId, CancellationToken cancellationToken = default);
        Task AddFactionAsync(int guildId, int factionId, CancellationToken cancellationToken = default);
        Task<bool> RemoveFactionAsync(int guildId, int factionId, CancellationToken cancellationToken = default);

        Task<bool> IsProfessionLinkedAsync(int guildId, int professionId, CancellationToken cancellationToken = default);
        Task AddProfessionAsync(int guildId, int professionId, CancellationToken cancellationToken = default);
        Task<bool> RemoveProfessionAsync(int guildId, int professionId, CancellationToken cancellationToken = default);

        Task<bool> IsSocialClassLinkedAsync(int guildId, int socialClassId, CancellationToken cancellationToken = default);
        Task AddSocialClassAsync(int guildId, int socialClassId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSocialClassAsync(int guildId, int socialClassId, CancellationToken cancellationToken = default);
    }
}
