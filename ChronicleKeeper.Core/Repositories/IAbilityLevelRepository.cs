using ChronicleKeeper.Core.Entities.Characters.Abilities;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IAbilityLevelRepository
    {
        Task<AbilityLevel> CreateAsync(AbilityLevel abilityLevel, CancellationToken cancellationToken = default);
        Task<AbilityLevel?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<AbilityLevel>> GetAllAsync(int? worldId = null, int? abilityId = null, CancellationToken cancellationToken = default);
        Task<AbilityLevel> UpdateAsync(AbilityLevel abilityLevel, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
