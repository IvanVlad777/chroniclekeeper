using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IArchitectureStyleRepository
    {
        Task<ArchitectureStyle> CreateAsync(ArchitectureStyle entity, CancellationToken ct = default);
        Task<ArchitectureStyle?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ArchitectureStyle?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<ArchitectureStyle>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<ArchitectureStyle> UpdateAsync(ArchitectureStyle entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> LocationExistsInWorldAsync(int locationId, int worldId, CancellationToken ct = default);
        Task AddLocationAsync(int styleId, int locationId, CancellationToken ct = default);
        Task RemoveLocationAsync(int styleId, int locationId, CancellationToken ct = default);
    }

    public interface IFolkloreRepository
    {
        Task<Folklore> CreateAsync(Folklore entity, CancellationToken ct = default);
        Task<Folklore?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Folklore?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Folklore>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Folklore> UpdateAsync(Folklore entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> EventExistsInWorldAsync(int eventId, int worldId, CancellationToken ct = default);
        Task AddEventAsync(int folkloreId, int eventId, CancellationToken ct = default);
        Task RemoveEventAsync(int folkloreId, int eventId, CancellationToken ct = default);

        Task<bool> SpeciesExistsInWorldAsync(int speciesId, int worldId, CancellationToken ct = default);
        Task AddSpeciesAsync(int folkloreId, int speciesId, CancellationToken ct = default);
        Task RemoveSpeciesAsync(int folkloreId, int speciesId, CancellationToken ct = default);
    }

    public interface IMythRepository
    {
        Task<Myth> CreateAsync(Myth entity, CancellationToken ct = default);
        Task<Myth?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Myth?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Myth>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Myth> UpdateAsync(Myth entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface ICulturalFestivalRepository
    {
        Task<CulturalFestival> CreateAsync(CulturalFestival entity, CancellationToken ct = default);
        Task<CulturalFestival?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<CulturalFestival?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<CulturalFestival>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<CulturalFestival> UpdateAsync(CulturalFestival entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface ICulturalInstitutionRepository
    {
        Task<CulturalInstitution> CreateAsync(CulturalInstitution entity, CancellationToken ct = default);
        Task<CulturalInstitution?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<CulturalInstitution?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<CulturalInstitution>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<CulturalInstitution> UpdateAsync(CulturalInstitution entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> CityExistsInWorldAsync(int cityId, int worldId, CancellationToken ct = default);

        // Notable artists (CulturalInstitutionArtist join with Character)
        Task<bool> IsArtistLinkedAsync(int institutionId, int characterId, CancellationToken ct = default);
        Task AddArtistAsync(int institutionId, int characterId, CancellationToken ct = default);
        Task<bool> RemoveArtistAsync(int institutionId, int characterId, CancellationToken ct = default);
    }
}
