using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Repositories
{
    /// <summary>Pokriva timelinee i njihove evente (jedan agregat).</summary>
    public interface ITimelineRepository
    {
        Task<Timeline> CreateAsync(Timeline timeline, CancellationToken cancellationToken = default);
        /// <summary>S eventima poredanim po SortOrder — za detail prikaz.</summary>
        Task<Timeline?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Timeline?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Timeline>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Timeline> UpdateAsync(Timeline timeline, CancellationToken cancellationToken = default);
        /// <summary>Briše timeline i sve njegove evente (DB cascade).</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        // Events
        Task<TimelineEvent> CreateEventAsync(TimelineEvent timelineEvent, CancellationToken cancellationToken = default);
        Task<TimelineEvent?> FindEventByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TimelineEvent> UpdateEventAsync(TimelineEvent timelineEvent, CancellationToken cancellationToken = default);
        Task<bool> DeleteEventAsync(int id, CancellationToken cancellationToken = default);
    }
}
