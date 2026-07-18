using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICustomRepository
    {
        Task<Custom> CreateAsync(Custom entity, CancellationToken ct = default);
        Task<Custom?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Custom?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Custom>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Custom> UpdateAsync(Custom entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface IArtFormRepository
    {
        Task<ArtForm> CreateAsync(ArtForm entity, CancellationToken ct = default);
        Task<ArtForm?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ArtForm?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<ArtForm>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<ArtForm> UpdateAsync(ArtForm entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface ICuisineRepository
    {
        Task<Cuisine> CreateAsync(Cuisine entity, CancellationToken ct = default);
        Task<Cuisine?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Cuisine?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Cuisine>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Cuisine> UpdateAsync(Cuisine entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface IClothingRepository
    {
        Task<Clothing> CreateAsync(Clothing entity, CancellationToken ct = default);
        Task<Clothing?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Clothing?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Clothing>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Clothing> UpdateAsync(Clothing entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface ITraditionRepository
    {
        Task<Tradition> CreateAsync(Tradition entity, CancellationToken ct = default);
        Task<Tradition?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Tradition?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Tradition>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Tradition> UpdateAsync(Tradition entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
