using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class TimelineRepository : ITimelineRepository
    {
        private readonly ApplicationDbContext _context;

        public TimelineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Timeline> CreateAsync(Timeline timeline, CancellationToken cancellationToken = default)
        {
            _context.Timelines.Add(timeline);
            await _context.SaveChangesAsync(cancellationToken);
            return timeline;
        }

        public async Task<Timeline?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Timelines
                .Include(t => t.Events.OrderBy(e => e.SortOrder))
                    .ThenInclude(e => e.Location)
                .Include(t => t.Events.OrderBy(e => e.SortOrder))
                    .ThenInclude(e => e.InvolvedCharacters)
                        .ThenInclude(ic => ic.Character)
                .Include(t => t.History)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<Timeline?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Timelines
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<Timeline>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Timelines.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(t => t.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Timeline> UpdateAsync(Timeline timeline, CancellationToken cancellationToken = default)
        {
            _context.Entry(timeline).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return timeline;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Events kaskadira DB
            var deleted = await _context.Timelines
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<TimelineEvent> CreateEventAsync(TimelineEvent timelineEvent, IEnumerable<int> involvedCharacterIds, CancellationToken cancellationToken = default)
        {
            await NormalizeLinksAsync(timelineEvent, cancellationToken);
            var validIds = await ValidCharacterIdsAsync(involvedCharacterIds, timelineEvent.WorldId, cancellationToken);
            timelineEvent.InvolvedCharacters = validIds
                .Select(cid => new TimelineEventCharacter { CharacterId = cid })
                .ToList();

            _context.TimelineEvents.Add(timelineEvent);
            await _context.SaveChangesAsync(cancellationToken);
            return timelineEvent;
        }

        public async Task<TimelineEvent?> FindEventByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.TimelineEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<TimelineEvent> UpdateEventAsync(TimelineEvent timelineEvent, IEnumerable<int> involvedCharacterIds, CancellationToken cancellationToken = default)
        {
            var existing = await _context.TimelineEvents
                .Include(e => e.InvolvedCharacters)
                .FirstOrDefaultAsync(e => e.Id == timelineEvent.Id, cancellationToken);
            if (existing == null) return timelineEvent;

            existing.Name = timelineEvent.Name;
            existing.Date = timelineEvent.Date;
            existing.SortOrder = timelineEvent.SortOrder;
            existing.Era = timelineEvent.Era;
            existing.Description = timelineEvent.Description;
            existing.Consequences = timelineEvent.Consequences;
            existing.IsMajorEvent = timelineEvent.IsMajorEvent;
            existing.LocationId = timelineEvent.LocationId;
            await NormalizeLinksAsync(existing, cancellationToken);

            var want = (await ValidCharacterIdsAsync(involvedCharacterIds, existing.WorldId, cancellationToken)).ToHashSet();
            foreach (var link in existing.InvolvedCharacters.Where(j => !want.Contains(j.CharacterId)).ToList())
            {
                existing.InvolvedCharacters.Remove(link);
            }
            var have = existing.InvolvedCharacters.Select(j => j.CharacterId).ToHashSet();
            foreach (var cid in want.Where(cid => !have.Contains(cid)))
            {
                existing.InvolvedCharacters.Add(new TimelineEventCharacter { CharacterId = cid });
            }

            await _context.SaveChangesAsync(cancellationToken);
            return existing;
        }

        /// <summary>Nulls a LocationId that isn't in the event's world (keeps the FK clean).</summary>
        private async Task NormalizeLinksAsync(TimelineEvent ev, CancellationToken ct)
        {
            if (ev.LocationId is int locId &&
                !await _context.Locations.AnyAsync(l => l.Id == locId && l.WorldId == ev.WorldId, ct))
            {
                ev.LocationId = null;
            }
        }

        private async Task<List<int>> ValidCharacterIdsAsync(IEnumerable<int> ids, int worldId, CancellationToken ct)
        {
            var idList = ids?.Distinct().ToList() ?? new List<int>();
            if (idList.Count == 0) return new List<int>();
            return await _context.Characters
                .Where(c => idList.Contains(c.Id) && c.WorldId == worldId)
                .Select(c => c.Id)
                .ToListAsync(ct);
        }

        public async Task<bool> DeleteEventAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.TimelineEvents
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
