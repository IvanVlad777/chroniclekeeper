using ChronicleKeeper.Core.Entities.Characters.Abilities;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IAbilityRepository
    {
        Task<Ability> CreateAsync(Ability ability, CancellationToken cancellationToken = default);
        Task<Ability?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Ability>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Ability> UpdateAsync(Ability ability, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
