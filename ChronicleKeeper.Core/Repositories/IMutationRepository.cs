using ChronicleKeeper.Core.Entities.Miscellaneous;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IMutationRepository
    {
        Task<Mutation> CreateAsync(Mutation mutation, CancellationToken cancellationToken = default);
        /// <summary>With mutant creature and history — for detail view.</summary>
        Task<Mutation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Mutation?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Mutation>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Mutation> UpdateAsync(Mutation mutation, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
