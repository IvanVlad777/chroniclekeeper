using ChronicleKeeper.Core.DTOs.History;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public HistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<History> CreateAsync(History history, CancellationToken cancellationToken = default)
        {
            _context.Histories.Add(history);
            await _context.SaveChangesAsync(cancellationToken);
            return history;
        }

        public async Task<History?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Histories
                .Include(h => h.Timelines)
                    .ThenInclude(t => t.Events)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<HistoryLinkDto>> GetLinkedEntitiesAsync(int historyId, CancellationToken cancellationToken = default)
        {
            // History has no navigation back to its owners — gather them per type.
            // Covers the four "major vertical" types shown on the hub; more can be
            // added here as other entities start surfacing their History link in the UI.
            var characters = await _context.Characters
                .Where(c => c.HistoryId == historyId)
                .Select(c => new HistoryLinkDto { Type = "Character", Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);

            var locations = await _context.Locations
                .Where(l => l.HistoryId == historyId)
                .Select(l => new HistoryLinkDto { Type = "Location", Id = l.Id, Name = l.Name })
                .ToListAsync(cancellationToken);

            var factions = await _context.Factions
                .Where(f => f.HistoryId == historyId)
                .Select(f => new HistoryLinkDto { Type = "Faction", Id = f.Id, Name = f.Name })
                .ToListAsync(cancellationToken);

            var nations = await _context.Nations
                .Where(n => n.HistoryId == historyId)
                .Select(n => new HistoryLinkDto { Type = "Nation", Id = n.Id, Name = n.Name })
                .ToListAsync(cancellationToken);

            return characters
                .Concat(locations)
                .Concat(factions)
                .Concat(nations)
                .ToList();
        }

        public async Task<bool> TargetExistsInWorldAsync(HistoryLinkTargetType targetType, int targetId, int worldId, CancellationToken cancellationToken = default)
        {
            return targetType switch
            {
                HistoryLinkTargetType.Character => await _context.Characters.AnyAsync(c => c.Id == targetId && c.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Location => await _context.Locations.AnyAsync(l => l.Id == targetId && l.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Faction => await _context.Factions.AnyAsync(f => f.Id == targetId && f.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Nation => await _context.Nations.AnyAsync(n => n.Id == targetId && n.WorldId == worldId, cancellationToken),
                _ => false,
            };
        }

        public async Task<int> SetTargetHistoryAsync(HistoryLinkTargetType targetType, int targetId, int? historyId, int? onlyIfCurrentHistoryId = null, CancellationToken cancellationToken = default)
        {
            // ExecuteUpdate keeps this a single UPDATE; the optional guard makes unlink
            // no-op unless the target currently points at the expected history.
            return targetType switch
            {
                HistoryLinkTargetType.Character => await _context.Characters
                    .Where(c => c.Id == targetId && (onlyIfCurrentHistoryId == null || c.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Location => await _context.Locations
                    .Where(l => l.Id == targetId && (onlyIfCurrentHistoryId == null || l.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(l => l.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Faction => await _context.Factions
                    .Where(f => f.Id == targetId && (onlyIfCurrentHistoryId == null || f.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(f => f.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Nation => await _context.Nations
                    .Where(n => n.Id == targetId && (onlyIfCurrentHistoryId == null || n.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(n => n.HistoryId, historyId), cancellationToken),
                _ => 0,
            };
        }

        public async Task<History?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Histories
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<History>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Histories.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(h => h.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<History> UpdateAsync(History history, CancellationToken cancellationToken = default)
        {
            _context.Entry(history).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return history;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Histories
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
