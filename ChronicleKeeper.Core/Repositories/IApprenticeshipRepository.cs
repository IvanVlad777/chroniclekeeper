using ChronicleKeeper.Core.Entities.Professions;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IApprenticeshipRepository
    {
        Task<Apprenticeship> CreateAsync(Apprenticeship apprenticeship, CancellationToken cancellationToken = default);
        /// <summary>S uključenom trgovačkom školom — za listu/detail.</summary>
        Task<Apprenticeship?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Apprenticeship?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Apprenticeship>> GetAllAsync(int? worldId = null, int? professionId = null, CancellationToken cancellationToken = default);
        Task<Apprenticeship> UpdateAsync(Apprenticeship apprenticeship, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
