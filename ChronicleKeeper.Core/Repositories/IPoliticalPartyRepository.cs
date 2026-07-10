using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IPoliticalPartyRepository
    {
        Task<PoliticalParty> CreateAsync(PoliticalParty party, CancellationToken cancellationToken = default);
        /// <summary>With ideology and government system included — for detail view.</summary>
        Task<PoliticalParty?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PoliticalParty?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<PoliticalParty>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<PoliticalParty> UpdateAsync(PoliticalParty party, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsFactionLinkedAsync(int politicalPartyId, int factionId, CancellationToken cancellationToken = default);
        Task AddFactionAsync(int politicalPartyId, int factionId, CancellationToken cancellationToken = default);
        Task<bool> RemoveFactionAsync(int politicalPartyId, int factionId, CancellationToken cancellationToken = default);

        Task<bool> IsNationLinkedAsync(int politicalPartyId, int nationId, CancellationToken cancellationToken = default);
        Task AddNationAsync(int politicalPartyId, int nationId, CancellationToken cancellationToken = default);
        Task<bool> RemoveNationAsync(int politicalPartyId, int nationId, CancellationToken cancellationToken = default);
    }
}
