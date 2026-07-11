using ChronicleKeeper.Core.Entities.Professions;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IProfessionRepository
    {
        Task<Profession> CreateAsync(Profession profession, CancellationToken cancellationToken = default);
        /// <summary>S uključenim rangovima, naukovanjima, specijalizacijama i cross-link kolekcijama — za detail prikaz.</summary>
        Task<Profession?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Profession?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Profession>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Profession> UpdateAsync(Profession profession, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountCharactersUsingProfessionAsync(int professionId, CancellationToken cancellationToken = default);

        Task<bool> IsSpeciesLinkedAsync(int professionId, int speciesId, CancellationToken cancellationToken = default);
        Task AddSpeciesAsync(int professionId, int speciesId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSpeciesAsync(int professionId, int speciesId, CancellationToken cancellationToken = default);

        Task<bool> IsSocialClassLinkedAsync(int professionId, int socialClassId, CancellationToken cancellationToken = default);
        Task AddSocialClassAsync(int professionId, int socialClassId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSocialClassAsync(int professionId, int socialClassId, CancellationToken cancellationToken = default);

        Task<bool> IsTradeSchoolLinkedAsync(int professionId, int tradeSchoolId, CancellationToken cancellationToken = default);
        Task AddTradeSchoolAsync(int professionId, int tradeSchoolId, CancellationToken cancellationToken = default);
        Task<bool> RemoveTradeSchoolAsync(int professionId, int tradeSchoolId, CancellationToken cancellationToken = default);
    }
}
