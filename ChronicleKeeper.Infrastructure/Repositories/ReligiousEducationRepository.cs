using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ReligiousEducationRepository : IReligiousEducationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReligiousEducationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReligiousEducation> CreateAsync(ReligiousEducation religiousEducation, CancellationToken cancellationToken = default)
        {
            _context.ReligiousEducations.Add(religiousEducation);
            await _context.SaveChangesAsync(cancellationToken);
            return religiousEducation;
        }

        public async Task<ReligiousEducation?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ReligiousEducations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<ReligiousEducation>> GetAllAsync(int? worldId = null, int? characterId = null, int? religionId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.ReligiousEducations.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(r => r.WorldId == wid);
            }
            if (characterId is int cid)
            {
                query = query.Where(r => r.CharacterId == cid);
            }
            if (religionId is int rid)
            {
                query = query.Where(r => r.ReligionId == rid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<ReligiousEducation> UpdateAsync(ReligiousEducation religiousEducation, CancellationToken cancellationToken = default)
        {
            _context.Entry(religiousEducation).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return religiousEducation;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ReligiousEducations
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
