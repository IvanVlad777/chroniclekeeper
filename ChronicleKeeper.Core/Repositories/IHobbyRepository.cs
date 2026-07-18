using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IHobbyRepository
    {
        Task<Hobby> CreateAsync(Hobby hobby, CancellationToken cancellationToken = default);
        /// <summary>With history — for detail view.</summary>
        Task<Hobby?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Hobby?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Hobby>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Hobby> UpdateAsync(Hobby hobby, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
