using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICharacterRepository
    {
        Task<Character> CreateAsync(Character character, CancellationToken cancellationToken = default);
        Task<Character?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Character>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Character> UpdateAsync(Character character, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
