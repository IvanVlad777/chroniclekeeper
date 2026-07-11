using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class EducationRecordRepository : IEducationRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public EducationRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EducationRecord> CreateAsync(EducationRecord record, CancellationToken cancellationToken = default)
        {
            _context.EducationRecords.Add(record);
            await _context.SaveChangesAsync(cancellationToken);
            return record;
        }

        public async Task<EducationRecord?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.EducationRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<EducationRecord>> GetAllAsync(int? worldId = null, int? characterId = null, int? schoolId = null, int? universityId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.EducationRecords.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(e => e.WorldId == wid);
            }
            if (characterId is int cid)
            {
                query = query.Where(e => e.CharacterId == cid);
            }
            if (schoolId is int sid)
            {
                query = query.Where(e => e.SchoolId == sid);
            }
            if (universityId is int uid)
            {
                query = query.Where(e => e.UniversityId == uid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<EducationRecord> UpdateAsync(EducationRecord record, CancellationToken cancellationToken = default)
        {
            _context.Entry(record).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return record;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.EducationRecords
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
