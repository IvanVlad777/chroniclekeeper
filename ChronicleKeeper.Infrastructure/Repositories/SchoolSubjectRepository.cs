using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class SchoolSubjectRepository : ISchoolSubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SchoolSubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SchoolSubject> CreateAsync(SchoolSubject subject, CancellationToken cancellationToken = default)
        {
            _context.SchoolSubjects.Add(subject);
            await _context.SaveChangesAsync(cancellationToken);
            return subject;
        }

        public async Task<SchoolSubject?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SchoolSubjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<SchoolSubject>> GetAllAsync(int? worldId = null, int? schoolId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.SchoolSubjects.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(s => s.WorldId == wid);
            }
            if (schoolId is int sid)
            {
                query = query.Where(s => s.SchoolId == sid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<SchoolSubject> UpdateAsync(SchoolSubject subject, CancellationToken cancellationToken = default)
        {
            _context.Entry(subject).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return subject;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.SchoolSubjects
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
