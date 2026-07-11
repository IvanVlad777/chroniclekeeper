using ChronicleKeeper.Core.Entities.Content.Movie;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IEpisodeRepository
    {
        Task<Episode> CreateAsync(Episode episode, CancellationToken cancellationToken = default);
        Task<Episode?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Episode>> GetAllAsync(int? worldId = null, int? seriesId = null, CancellationToken cancellationToken = default);
        Task<Episode> UpdateAsync(Episode episode, CancellationToken cancellationToken = default);

        /// <summary>Vraća false ako epizoda ne postoji. Prije brisanja čisti References koje ciljaju
        /// ovu epizodu (Reference.EpisodeId je Restrict).</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
