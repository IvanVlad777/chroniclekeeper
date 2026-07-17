using ChronicleKeeper.Core.Entities.Social.Military;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IMilitaryDoctrineRepository
    {
        Task<MilitaryDoctrine> CreateAsync(MilitaryDoctrine entity, CancellationToken ct = default);
        Task<MilitaryDoctrine?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<MilitaryDoctrine?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<MilitaryDoctrine>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<MilitaryDoctrine> UpdateAsync(MilitaryDoctrine entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface IMilitaryOrganizationRepository
    {
        Task<MilitaryOrganization> CreateAsync(MilitaryOrganization entity, CancellationToken ct = default);
        Task<MilitaryOrganization?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<MilitaryOrganization?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<MilitaryOrganization>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<MilitaryOrganization> UpdateAsync(MilitaryOrganization entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> CountryExistsInWorldAsync(int countryId, int worldId, CancellationToken ct = default);
        Task<bool> FactionExistsInWorldAsync(int factionId, int worldId, CancellationToken ct = default);
        Task AddCountryAsync(int organizationId, int countryId, CancellationToken ct = default);
        Task RemoveCountryAsync(int organizationId, int countryId, CancellationToken ct = default);
        Task AddFactionAsync(int organizationId, int factionId, CancellationToken ct = default);
        Task RemoveFactionAsync(int organizationId, int factionId, CancellationToken ct = default);
    }

    public interface IArmyRepository
    {
        Task<Army> CreateAsync(Army entity, CancellationToken ct = default);
        Task<Army?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Army?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Army>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Army> UpdateAsync(Army entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> BattleExistsInWorldAsync(int battleId, int worldId, CancellationToken ct = default);
        Task AddBattleAsync(int armyId, int battleId, CancellationToken ct = default);
        Task RemoveBattleAsync(int armyId, int battleId, CancellationToken ct = default);
    }

    public interface IMilitaryUnitRepository
    {
        Task<MilitaryUnit> CreateAsync(MilitaryUnit entity, CancellationToken ct = default);
        Task<MilitaryUnit?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<MilitaryUnit?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<MilitaryUnit>> GetAllAsync(int? worldId = null, int? armyId = null, CancellationToken ct = default);
        Task<MilitaryUnit> UpdateAsync(MilitaryUnit entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> EquipmentExistsInWorldAsync(int equipmentId, int worldId, CancellationToken ct = default);
        Task AddEquipmentAsync(int unitId, int equipmentId, CancellationToken ct = default);
        Task RemoveEquipmentAsync(int unitId, int equipmentId, CancellationToken ct = default);
    }

    public interface IMilitaryRankRepository
    {
        Task<MilitaryRank> CreateAsync(MilitaryRank entity, CancellationToken ct = default);
        Task<MilitaryRank?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<MilitaryRank?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<MilitaryRank>> GetAllAsync(int? worldId = null, int? unitId = null, CancellationToken ct = default);
        Task<MilitaryRank> UpdateAsync(MilitaryRank entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface IBattleRepository
    {
        Task<Battle> CreateAsync(Battle entity, CancellationToken ct = default);
        Task<Battle?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Battle?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Battle>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Battle> UpdateAsync(Battle entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface IMilitaryEquipmentRepository
    {
        Task<MilitaryEquipment> CreateAsync(MilitaryEquipment entity, CancellationToken ct = default);
        Task<MilitaryEquipment?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<MilitaryEquipment?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<MilitaryEquipment>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<MilitaryEquipment> UpdateAsync(MilitaryEquipment entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
