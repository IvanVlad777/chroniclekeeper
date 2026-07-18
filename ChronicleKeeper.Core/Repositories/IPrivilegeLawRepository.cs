using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IPrivilegeLawRepository
    {
        Task<PrivilegeLaw> CreateAsync(PrivilegeLaw law, CancellationToken cancellationToken = default);
        Task<PrivilegeLaw?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<PrivilegeLaw>> GetAllAsync(int? worldId = null, int? socialClassId = null, CancellationToken cancellationToken = default);
        Task<PrivilegeLaw> UpdateAsync(PrivilegeLaw law, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
