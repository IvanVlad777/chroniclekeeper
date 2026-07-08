using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Repositories
{
    /// <summary>Pokriva SapientSpecies i njihove Race (jedan agregat).</summary>
    public interface ISpeciesRepository
    {
        // Species
        Task<SapientSpecies> CreateAsync(SapientSpecies species, CancellationToken cancellationToken = default);
        /// <summary>S uključenim rasama — za detail prikaz.</summary>
        Task<SapientSpecies?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<SapientSpecies?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<SapientSpecies>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<SapientSpecies> UpdateAsync(SapientSpecies species, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Broj likova koji koriste vrstu ili bilo koju njenu rasu.</summary>
        Task<int> CountCharactersUsingSpeciesAsync(int speciesId, CancellationToken cancellationToken = default);

        // Races
        Task<Race> CreateRaceAsync(Race race, CancellationToken cancellationToken = default);
        Task<Race?> FindRaceByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Race>> GetRacesAsync(int? worldId = null, int? speciesId = null, CancellationToken cancellationToken = default);
        Task<Race> UpdateRaceAsync(Race race, CancellationToken cancellationToken = default);
        Task<bool> DeleteRaceAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountCharactersUsingRaceAsync(int raceId, CancellationToken cancellationToken = default);
    }
}
