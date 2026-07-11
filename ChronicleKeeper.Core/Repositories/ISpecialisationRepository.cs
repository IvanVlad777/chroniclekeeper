using ChronicleKeeper.Core.Entities.Professions;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ISpecialisationRepository
    {
        Task<Specialisation> CreateAsync(Specialisation specialisation, CancellationToken cancellationToken = default);
        Task<Specialisation?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Specialisation>> GetAllAsync(int? worldId = null, int? professionId = null, CancellationToken cancellationToken = default);
        Task<Specialisation> UpdateAsync(Specialisation specialisation, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
