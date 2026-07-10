using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly ApplicationDbContext _context;

        public LanguageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Language> CreateAsync(Language language, CancellationToken cancellationToken = default)
        {
            _context.Languages.Add(language);
            await _context.SaveChangesAsync(cancellationToken);
            return language;
        }

        public async Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Languages
                .Include(l => l.Cultures)
                .Include(l => l.Nations).ThenInclude(ln => ln.Nation)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<Language?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Languages
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<List<Language>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Languages.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(l => l.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Language> UpdateAsync(Language language, CancellationToken cancellationToken = default)
        {
            _context.Entry(language).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return language;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Languages
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountCulturesUsingLanguageAsync(int languageId, CancellationToken cancellationToken = default)
        {
            return await _context.Cultures.CountAsync(c => c.LanguageId == languageId, cancellationToken);
        }

        public async Task<bool> IsNationLinkedAsync(int languageId, int nationId, CancellationToken cancellationToken = default)
        {
            return await _context.LanguageNations
                .AnyAsync(ln => ln.LanguageId == languageId && ln.NationId == nationId, cancellationToken);
        }

        public async Task AddNationAsync(int languageId, int nationId, CancellationToken cancellationToken = default)
        {
            _context.LanguageNations.Add(new LanguageNation { LanguageId = languageId, NationId = nationId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveNationAsync(int languageId, int nationId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.LanguageNations
                .Where(ln => ln.LanguageId == languageId && ln.NationId == nationId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
