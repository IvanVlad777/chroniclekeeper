using ChronicleKeeper.Core.Entities.Social;

namespace ChronicleKeeper.Core.Repositories
{
    /// <summary>Pokriva frakcije i njihova članstva (jedan agregat).</summary>
    public interface IFactionRepository
    {
        Task<Faction> CreateAsync(Faction faction, CancellationToken cancellationToken = default);
        /// <summary>Puni graf (leader, HQ, članovi, tagovi) — za detail prikaz.</summary>
        Task<Faction?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Faction?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Faction>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Faction> UpdateAsync(Faction faction, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        // Members
        Task<FactionMember> AddMemberAsync(FactionMember member, CancellationToken cancellationToken = default);
        Task<bool> RemoveMemberAsync(int factionId, int characterId, CancellationToken cancellationToken = default);
        Task<bool> IsMemberAsync(int factionId, int characterId, CancellationToken cancellationToken = default);
    }
}
