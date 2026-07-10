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
    }
}
