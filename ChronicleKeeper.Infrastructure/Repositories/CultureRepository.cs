using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class CultureRepository : ICultureRepository
    {
        private readonly ApplicationDbContext _context;

        public CultureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Culture> CreateAsync(Culture culture, CancellationToken cancellationToken = default)
        {
            _context.Cultures.Add(culture);
            await _context.SaveChangesAsync(cancellationToken);
            return culture;
        }

        public async Task<Culture?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Cultures
                .Include(c => c.Language)
                .Include(c => c.Religion)
                .Include(c => c.Nations).ThenInclude(cn => cn.Nation)
                .Include(c => c.PracticedBySpecies).ThenInclude(cs => cs.SapientSpecies)
                .Include(c => c.InfluencedSocialClasses).ThenInclude(cs => cs.SocialClass)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Culture?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Cultures
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Culture>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Cultures.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Culture> UpdateAsync(Culture culture, CancellationToken cancellationToken = default)
        {
            _context.Entry(culture).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return culture;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Cultures
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsNationLinkedAsync(int cultureId, int nationId, CancellationToken cancellationToken = default)
        {
            return await _context.CultureNations
                .AnyAsync(cn => cn.CultureId == cultureId && cn.NationId == nationId, cancellationToken);
        }

        public async Task AddNationAsync(int cultureId, int nationId, CancellationToken cancellationToken = default)
        {
            _context.CultureNations.Add(new CultureNation { CultureId = cultureId, NationId = nationId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveNationAsync(int cultureId, int nationId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CultureNations
                .Where(cn => cn.CultureId == cultureId && cn.NationId == nationId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsSapientSpeciesLinkedAsync(int cultureId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            return await _context.CultureSapientSpecies
                .AnyAsync(cs => cs.CultureId == cultureId && cs.SapientSpeciesId == sapientSpeciesId, cancellationToken);
        }

        public async Task AddSapientSpeciesAsync(int cultureId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            _context.CultureSapientSpecies.Add(new CultureSapientSpecies { CultureId = cultureId, SapientSpeciesId = sapientSpeciesId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveSapientSpeciesAsync(int cultureId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CultureSapientSpecies
                .Where(cs => cs.CultureId == cultureId && cs.SapientSpeciesId == sapientSpeciesId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsSocialClassLinkedAsync(int cultureId, int socialClassId, CancellationToken cancellationToken = default)
        {
            return await _context.CultureSocialClasses
                .AnyAsync(cs => cs.CultureId == cultureId && cs.SocialClassId == socialClassId, cancellationToken);
        }

        public async Task AddSocialClassAsync(int cultureId, int socialClassId, CancellationToken cancellationToken = default)
        {
            _context.CultureSocialClasses.Add(new CultureSocialClass { CultureId = cultureId, SocialClassId = socialClassId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveSocialClassAsync(int cultureId, int socialClassId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CultureSocialClasses
                .Where(cs => cs.CultureId == cultureId && cs.SocialClassId == socialClassId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
