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

        public async Task<SchoolSubject?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SchoolSubjects
                .Include(s => s.School)
                .Include(s => s.Teachers).ThenInclude(t => t.Character)
                .AsNoTracking()
                .AsSplitQuery()
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

        public async Task<bool> IsTeacherLinkedAsync(int subjectId, int characterId, CancellationToken cancellationToken = default)
        {
            return await _context.SchoolSubjectTeachers
                .AnyAsync(t => t.SchoolSubjectId == subjectId && t.CharacterId == characterId, cancellationToken);
        }

        public async Task AddTeacherAsync(int subjectId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.SchoolSubjectTeachers.Add(new SchoolSubjectTeacher { SchoolSubjectId = subjectId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveTeacherAsync(int subjectId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.SchoolSubjectTeachers
                .Where(t => t.SchoolSubjectId == subjectId && t.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
