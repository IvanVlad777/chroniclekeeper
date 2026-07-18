using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ReligiousOrderRepository : IReligiousOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public ReligiousOrderRepository(ApplicationDbContext context) => _context = context;

        public async Task<ReligiousOrder> CreateAsync(ReligiousOrder entity, CancellationToken ct = default)
        {
            _context.ReligiousOrders.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<ReligiousOrder?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.ReligiousOrders
                .Include(o => o.Religion)
                .Include(o => o.History)
                .Include(o => o.ClergyTraining)
                .Include(o => o.Deities).ThenInclude(dro => dro.Deity)
                .Include(o => o.Factions).ThenInclude(rof => rof.Faction)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, ct);

        public Task<ReligiousOrder?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.ReligiousOrders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, ct);

        public async Task<List<ReligiousOrder>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.ReligiousOrders.AsNoTracking();
            if (worldId is int w) q = q.Where(o => o.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<ReligiousOrder> UpdateAsync(ReligiousOrder entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            await _context.ReligiousOrders.Where(o => o.Id == id).ExecuteDeleteAsync(ct) > 0;

        public Task<bool> FactionExistsInWorldAsync(int factionId, int worldId, CancellationToken ct = default) =>
            _context.Factions.AnyAsync(f => f.Id == factionId && f.WorldId == worldId, ct);

        public async Task AddFactionAsync(int orderId, int factionId, CancellationToken ct = default)
        {
            if (!await _context.ReligiousOrderFactions.AnyAsync(x => x.ReligiousOrderId == orderId && x.FactionId == factionId, ct))
            {
                _context.ReligiousOrderFactions.Add(new ReligiousOrderFaction { ReligiousOrderId = orderId, FactionId = factionId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveFactionAsync(int orderId, int factionId, CancellationToken ct = default) =>
            await _context.ReligiousOrderFactions
                .Where(x => x.ReligiousOrderId == orderId && x.FactionId == factionId)
                .ExecuteDeleteAsync(ct);
    }

    public class DeityRepository : IDeityRepository
    {
        private readonly ApplicationDbContext _context;
        public DeityRepository(ApplicationDbContext context) => _context = context;

        public async Task<Deity> CreateAsync(Deity entity, CancellationToken ct = default)
        {
            _context.Deities.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Deity?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _context.Deities
                .Include(d => d.Religion)
                .Include(d => d.History)
                .Include(d => d.SacredTexts)
                .Include(d => d.SacredSitesOfDeity)
                .Include(d => d.MajorMyths)
                .Include(d => d.OrdersDedicatedToDeity).ThenInclude(dro => dro.ReligiousOrder)
                .Include(d => d.AlliedDeities).ThenInclude(da => da.AlliedDeity)
                .Include(d => d.RivalDeities).ThenInclude(dr => dr.RivalDeity)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id, ct);

        public Task<Deity?> FindByIdAsync(int id, CancellationToken ct = default) =>
            _context.Deities.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, ct);

        public async Task<List<Deity>> GetAllAsync(int? worldId = null, CancellationToken ct = default)
        {
            var q = _context.Deities.AsNoTracking();
            if (worldId is int w) q = q.Where(d => d.WorldId == w);
            return await q.ToListAsync(ct);
        }

        public async Task<Deity> UpdateAsync(Deity entity, CancellationToken ct = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            // Self-ref Restrict sides must be cleared before deleting the deity.
            // The DeityId side of both joins cascades automatically; the Allied/Rival
            // side is Restrict (two FKs to the same table — only one may cascade).
            await _context.DeityAlliances.Where(a => a.AlliedDeityId == id).ExecuteDeleteAsync(ct);
            await _context.DeityRivalries.Where(r => r.RivalDeityId == id).ExecuteDeleteAsync(ct);

            var deleted = await _context.Deities.Where(d => d.Id == id).ExecuteDeleteAsync(ct);

            await transaction.CommitAsync(ct);
            return deleted > 0;
        }

        public Task<bool> OrderExistsInWorldAsync(int orderId, int worldId, CancellationToken ct = default) =>
            _context.ReligiousOrders.AnyAsync(o => o.Id == orderId && o.WorldId == worldId, ct);

        public async Task AddOrderAsync(int deityId, int orderId, CancellationToken ct = default)
        {
            if (!await _context.DeityReligiousOrders.AnyAsync(x => x.DeityId == deityId && x.ReligiousOrderId == orderId, ct))
            {
                _context.DeityReligiousOrders.Add(new DeityReligiousOrder { DeityId = deityId, ReligiousOrderId = orderId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveOrderAsync(int deityId, int orderId, CancellationToken ct = default) =>
            await _context.DeityReligiousOrders
                .Where(x => x.DeityId == deityId && x.ReligiousOrderId == orderId)
                .ExecuteDeleteAsync(ct);

        public Task<bool> DeityExistsInWorldAsync(int deityId, int worldId, CancellationToken ct = default) =>
            _context.Deities.AnyAsync(d => d.Id == deityId && d.WorldId == worldId, ct);

        public Task<bool> AllianceExistsAsync(int deityId, int alliedDeityId, CancellationToken ct = default) =>
            _context.DeityAlliances.AnyAsync(a => a.DeityId == deityId && a.AlliedDeityId == alliedDeityId, ct);

        public async Task AddAllyAsync(int deityId, int alliedDeityId, CancellationToken ct = default)
        {
            if (!await _context.DeityAlliances.AnyAsync(a => a.DeityId == deityId && a.AlliedDeityId == alliedDeityId, ct))
            {
                _context.DeityAlliances.Add(new DeityAlliance { DeityId = deityId, AlliedDeityId = alliedDeityId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveAllyAsync(int deityId, int alliedDeityId, CancellationToken ct = default) =>
            await _context.DeityAlliances
                .Where(a => a.DeityId == deityId && a.AlliedDeityId == alliedDeityId)
                .ExecuteDeleteAsync(ct);

        public Task<bool> RivalryExistsAsync(int deityId, int rivalDeityId, CancellationToken ct = default) =>
            _context.DeityRivalries.AnyAsync(r => r.DeityId == deityId && r.RivalDeityId == rivalDeityId, ct);

        public async Task AddRivalAsync(int deityId, int rivalDeityId, CancellationToken ct = default)
        {
            if (!await _context.DeityRivalries.AnyAsync(r => r.DeityId == deityId && r.RivalDeityId == rivalDeityId, ct))
            {
                _context.DeityRivalries.Add(new DeityRivalry { DeityId = deityId, RivalDeityId = rivalDeityId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemoveRivalAsync(int deityId, int rivalDeityId, CancellationToken ct = default) =>
            await _context.DeityRivalries
                .Where(r => r.DeityId == deityId && r.RivalDeityId == rivalDeityId)
                .ExecuteDeleteAsync(ct);
    }
}
