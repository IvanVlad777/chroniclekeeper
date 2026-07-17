using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social.Military;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class MilitaryDoctrineRepository : IMilitaryDoctrineRepository
    {
        private readonly ApplicationDbContext _context;
        public MilitaryDoctrineRepository(ApplicationDbContext context) => _context = context;

        public async Task<MilitaryDoctrine> CreateAsync(MilitaryDoctrine entity, CancellationToken ct = default)
        {
            _context.MilitaryDoctrines.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<MilitaryDoctrine?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryDoctrines
                .Include(d => d.History)
                .Include(d => d.MilitaryOrganizationsUsing)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id, ct);

        public Task<MilitaryDoctrine?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryDoctrines.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, ct);

        public async Task<List<MilitaryDoctrine>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.MilitaryDoctrines.AsNoTracking();
            if (worldId is int w) q = q.Where(d => d.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<MilitaryDoctrine> UpdateAsync(MilitaryDoctrine entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.MilitaryDoctrines.Where(d => d.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class MilitaryOrganizationRepository : IMilitaryOrganizationRepository
    {
        private readonly ApplicationDbContext _context;
        public MilitaryOrganizationRepository(ApplicationDbContext context) => _context = context;

        public async Task<MilitaryOrganization> CreateAsync(MilitaryOrganization entity, CancellationToken ct = default)
        {
            _context.MilitaryOrganizations.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<MilitaryOrganization?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryOrganizations
                .Include(o => o.History)
                .Include(o => o.MilitaryDoctrine)
                .Include(o => o.Armies)
                .Include(o => o.Countries).ThenInclude(c => c.Country)
                .Include(o => o.Factions).ThenInclude(f => f.Faction)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, ct);

        public Task<MilitaryOrganization?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryOrganizations.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, ct);

        public async Task<List<MilitaryOrganization>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.MilitaryOrganizations.AsNoTracking();
            if (worldId is int w) q = q.Where(o => o.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<MilitaryOrganization> UpdateAsync(MilitaryOrganization entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.MilitaryOrganizations.Where(o => o.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> CountryExistsInWorldAsync(int countryId, int worldId, CancellationToken ct = default) =>
            _context.Locations.OfType<Country>().AnyAsync(c => c.Id == countryId && c.WorldId == worldId, ct);

        public Task<bool> FactionExistsInWorldAsync(int factionId, int worldId, CancellationToken ct = default) =>
            _context.Factions.AnyAsync(f => f.Id == factionId && f.WorldId == worldId, ct);

        public async Task AddCountryAsync(int organizationId, int countryId, CancellationToken ct = default)
        {
            if (!await _context.MilitaryOrganizationCountries.AnyAsync(x => x.MilitaryOrganizationId == organizationId && x.CountryId == countryId, ct))
            {
                _context.MilitaryOrganizationCountries.Add(new MilitaryOrganizationCountry { MilitaryOrganizationId = organizationId, CountryId = countryId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveCountryAsync(int organizationId, int countryId, CancellationToken ct = default) =>
            await _context.MilitaryOrganizationCountries
                .Where(x => x.MilitaryOrganizationId == organizationId && x.CountryId == countryId)
                .ExecuteDeleteAsync(ct);

        public async Task AddFactionAsync(int organizationId, int factionId, CancellationToken ct = default)
        {
            if (!await _context.MilitaryOrganizationFactions.AnyAsync(x => x.MilitaryOrganizationId == organizationId && x.FactionId == factionId, ct))
            {
                _context.MilitaryOrganizationFactions.Add(new MilitaryOrganizationFaction { MilitaryOrganizationId = organizationId, FactionId = factionId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveFactionAsync(int organizationId, int factionId, CancellationToken ct = default) =>
            await _context.MilitaryOrganizationFactions
                .Where(x => x.MilitaryOrganizationId == organizationId && x.FactionId == factionId)
                .ExecuteDeleteAsync(ct);
    }

    public class ArmyRepository : IArmyRepository
    {
        private readonly ApplicationDbContext _context;
        public ArmyRepository(ApplicationDbContext context) => _context = context;

        public async Task<Army> CreateAsync(Army entity, CancellationToken ct = default)
        {
            _context.Armies.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Army?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Armies
                .Include(a => a.History)
                .Include(a => a.City)
                .Include(a => a.MilitaryOrganization)
                .Include(a => a.Faction)
                .Include(a => a.Units)
                .Include(a => a.Battles).ThenInclude(ab => ab.Battle)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, ct);

        public Task<Army?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Armies.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

        public async Task<List<Army>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Armies.AsNoTracking();
            if (worldId is int w) q = q.Where(a => a.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Army> UpdateAsync(Army entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Armies.Where(a => a.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> BattleExistsInWorldAsync(int battleId, int worldId, CancellationToken ct = default) =>
            _context.Battles.AnyAsync(b => b.Id == battleId && b.WorldId == worldId, ct);

        public async Task AddBattleAsync(int armyId, int battleId, CancellationToken ct = default)
        {
            if (!await _context.ArmyBattles.AnyAsync(x => x.ArmyId == armyId && x.BattleId == battleId, ct))
            {
                _context.ArmyBattles.Add(new ArmyBattle { ArmyId = armyId, BattleId = battleId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveBattleAsync(int armyId, int battleId, CancellationToken ct = default) =>
            await _context.ArmyBattles.Where(x => x.ArmyId == armyId && x.BattleId == battleId).ExecuteDeleteAsync(ct);
    }

    public class MilitaryUnitRepository : IMilitaryUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public MilitaryUnitRepository(ApplicationDbContext context) => _context = context;

        public async Task<MilitaryUnit> CreateAsync(MilitaryUnit entity, CancellationToken ct = default)
        {
            _context.MilitaryUnits.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<MilitaryUnit?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryUnits
                .Include(u => u.History)
                .Include(u => u.BelongsToArmy)
                .Include(u => u.Ranks)
                .Include(u => u.Equipment).ThenInclude(ue => ue.MilitaryEquipment)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);

        public Task<MilitaryUnit?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryUnits.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

        public async Task<List<MilitaryUnit>> GetAllAsync(int? worldId = null, int? armyId = null, CancellationToken ct = default)
        {
            var q = _context.MilitaryUnits.AsNoTracking();
            if (worldId is int w) q = q.Where(u => u.WorldId == w);
            if (armyId is int a) q = q.Where(u => u.BelongsToArmyId == a);
            return await q.ToListAsync(ct);
        }

        public async Task<MilitaryUnit> UpdateAsync(MilitaryUnit entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.MilitaryUnits.Where(u => u.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> EquipmentExistsInWorldAsync(int equipmentId, int worldId, CancellationToken ct = default) =>
            _context.MilitaryEquipments.AnyAsync(e => e.Id == equipmentId && e.WorldId == worldId, ct);

        public async Task AddEquipmentAsync(int unitId, int equipmentId, CancellationToken ct = default)
        {
            if (!await _context.MilitaryUnitEquipments.AnyAsync(x => x.MilitaryUnitId == unitId && x.MilitaryEquipmentId == equipmentId, ct))
            {
                _context.MilitaryUnitEquipments.Add(new MilitaryUnitEquipment { MilitaryUnitId = unitId, MilitaryEquipmentId = equipmentId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveEquipmentAsync(int unitId, int equipmentId, CancellationToken ct = default) =>
            await _context.MilitaryUnitEquipments.Where(x => x.MilitaryUnitId == unitId && x.MilitaryEquipmentId == equipmentId).ExecuteDeleteAsync(ct);
    }

    public class MilitaryRankRepository : IMilitaryRankRepository
    {
        private readonly ApplicationDbContext _context;
        public MilitaryRankRepository(ApplicationDbContext context) => _context = context;

        public async Task<MilitaryRank> CreateAsync(MilitaryRank entity, CancellationToken ct = default)
        {
            _context.MilitaryRanks.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<MilitaryRank?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryRanks.Include(r => r.History).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);

        public Task<MilitaryRank?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryRanks.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);

        public async Task<List<MilitaryRank>> GetAllAsync(int? worldId = null, int? unitId = null, CancellationToken ct = default)
        {
            var q = _context.MilitaryRanks.AsNoTracking();
            if (worldId is int w) q = q.Where(r => r.WorldId == w);
            if (unitId is int u) q = q.Where(r => r.MilitaryUnitId == u);
            return await q.OrderByDescending(r => r.RankLevel).ToListAsync(ct);
        }

        public async Task<MilitaryRank> UpdateAsync(MilitaryRank entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.MilitaryRanks.Where(r => r.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class BattleRepository : IBattleRepository
    {
        private readonly ApplicationDbContext _context;
        public BattleRepository(ApplicationDbContext context) => _context = context;

        public async Task<Battle> CreateAsync(Battle entity, CancellationToken ct = default)
        {
            _context.Battles.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Battle?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Battles
                .Include(b => b.History)
                .Include(b => b.ParticipatingArmies).ThenInclude(ab => ab.Army)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, ct);

        public Task<Battle?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Battles.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, ct);

        public async Task<List<Battle>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Battles.AsNoTracking();
            if (worldId is int w) q = q.Where(b => b.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Battle> UpdateAsync(Battle entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.Battles.Where(b => b.Id == id).ExecuteDeleteAsync(ct) > 0;
    }

    public class MilitaryEquipmentRepository : IMilitaryEquipmentRepository
    {
        private readonly ApplicationDbContext _context;
        public MilitaryEquipmentRepository(ApplicationDbContext context) => _context = context;

        public async Task<MilitaryEquipment> CreateAsync(MilitaryEquipment entity, CancellationToken ct = default)
        {
            _context.MilitaryEquipments.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<MilitaryEquipment?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryEquipments
                .Include(e => e.History)
                .Include(e => e.MilitaryUnits).ThenInclude(ue => ue.MilitaryUnit)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, ct);

        public Task<MilitaryEquipment?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.MilitaryEquipments.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<List<MilitaryEquipment>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.MilitaryEquipments.AsNoTracking();
            if (worldId is int w) q = q.Where(e => e.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<MilitaryEquipment> UpdateAsync(MilitaryEquipment entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.MilitaryEquipments.Where(e => e.Id == id).ExecuteDeleteAsync(ct) > 0;
    }
}
