using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IDiplomaticAgreementRepository
    {
        Task<DiplomaticAgreement> CreateAsync(DiplomaticAgreement agreement, CancellationToken cancellationToken = default);
        /// <summary>With both signatory nations included — for detail view.</summary>
        Task<DiplomaticAgreement?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<DiplomaticAgreement?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<DiplomaticAgreement>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<DiplomaticAgreement> UpdateAsync(DiplomaticAgreement agreement, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
