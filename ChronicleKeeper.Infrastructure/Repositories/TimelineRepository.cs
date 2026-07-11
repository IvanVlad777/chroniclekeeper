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

        public async Task<TimelineEvent> CreateEventAsync(TimelineEvent timelineEvent, CancellationToken cancellationToken = default)
        {
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

        public async Task<TimelineEvent> UpdateEventAsync(TimelineEvent timelineEvent, CancellationToken cancellationToken = default)
        {
            _context.Entry(timelineEvent).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return timelineEvent;
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
