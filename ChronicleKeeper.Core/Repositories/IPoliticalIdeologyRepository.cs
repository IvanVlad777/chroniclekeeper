using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IPoliticalIdeologyRepository
    {
        Task<PoliticalIdeology> CreateAsync(PoliticalIdeology ideology, CancellationToken cancellationToken = default);
        /// <summary>With affiliated parties/government systems included — for detail view.</summary>
        Task<PoliticalIdeology?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PoliticalIdeology?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<PoliticalIdeology>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<PoliticalIdeology> UpdateAsync(PoliticalIdeology ideology, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountPoliticalPartiesUsingIdeologyAsync(int ideologyId, CancellationToken cancellationToken = default);
        Task<int> CountGovernmentSystemsUsingIdeologyAsync(int ideologyId, CancellationToken cancellationToken = default);
    }
}
