using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICharacterRepository
    {
        Task<Character> CreateAsync(Character character, CancellationToken cancellationToken = default);
        /// <summary>Puni graf (obitelj, vrsta/rasa, veze, frakcije, tagovi) — za detail prikaz.</summary>
        Task<Character?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Samo korijenski entitet, bez Include-ova — za update/interne provjere.</summary>
        Task<Character?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Character>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Character> UpdateAsync(Character character, CancellationToken cancellationToken = default);
        /// <summary>Vraća false ako lik ne postoji.</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Postoji li lik u zadanom svijetu (za validaciju cross-world referenci).</summary>
        Task<bool> ExistsInWorldAsync(int characterId, int worldId, CancellationToken cancellationToken = default);
        /// <summary>Vraća SapientSpeciesId rase unutar zadanog svijeta, ili null ako rasa ne postoji u tom svijetu.</summary>
        Task<int?> GetSpeciesIdForRaceAsync(int raceId, int worldId, CancellationToken cancellationToken = default);
        /// <summary>Postoji li vrsta u zadanom svijetu.</summary>
        Task<bool> SpeciesExistsInWorldAsync(int speciesId, int worldId, CancellationToken cancellationToken = default);
    }
}
