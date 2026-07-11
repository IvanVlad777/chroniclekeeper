using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IReligiousEducationRepository
    {
        Task<ReligiousEducation> CreateAsync(ReligiousEducation religiousEducation, CancellationToken cancellationToken = default);
        Task<ReligiousEducation?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ReligiousEducation>> GetAllAsync(int? worldId = null, int? characterId = null, int? religionId = null, CancellationToken cancellationToken = default);
        Task<ReligiousEducation> UpdateAsync(ReligiousEducation religiousEducation, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
