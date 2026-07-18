using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly ApplicationDbContext _context;

        public SchoolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<School> CreateAsync(School school, CancellationToken cancellationToken = default)
        {
            _context.Schools.Add(school);
            await _context.SaveChangesAsync(cancellationToken);
            return school;
        }

        public async Task<School?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // _context.Schools (base DbSet) vraća i School i TradeSchool retke (TPH) — namjerno.
            return await _context.Schools
                .Include(s => s.Subjects)
                .Include(s => s.Alumni)
                .Include(s => s.Location)
                .Include(s => s.Students).ThenInclude(ss => ss.Character)
                .Include(s => s.Teachers).ThenInclude(st => st.Character)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<School?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Schools
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<School>> GetAllAsync(int? worldId = null, int? educationSystemId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Schools.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(s => s.WorldId == wid);
            }
            if (educationSystemId is int esid)
            {
                query = query.Where(s => s.EducationSystemId == esid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<School> UpdateAsync(School school, CancellationToken cancellationToken = default)
        {
            _context.Entry(school).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return school;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Subjects kaskadira DB; EducationRecords je handler već provjerio (Restrict).
            var deleted = await _context.Schools
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountEducationRecordsUsingSchoolAsync(int schoolId, CancellationToken cancellationToken = default)
        {
            return await _context.EducationRecords.CountAsync(e => e.SchoolId == schoolId, cancellationToken);
        }

        public Task<bool> IsStudentLinkedAsync(int schoolId, int characterId, CancellationToken cancellationToken = default)
            => _context.SchoolStudents.AnyAsync(x => x.SchoolId == schoolId && x.CharacterId == characterId, cancellationToken);

        public async Task AddStudentAsync(int schoolId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.SchoolStudents.Add(new SchoolStudent { SchoolId = schoolId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveStudentAsync(int schoolId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.SchoolStudents
                .Where(x => x.SchoolId == schoolId && x.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public Task<bool> IsTeacherLinkedAsync(int schoolId, int characterId, CancellationToken cancellationToken = default)
            => _context.SchoolTeachers.AnyAsync(x => x.SchoolId == schoolId && x.CharacterId == characterId, cancellationToken);

        public async Task AddTeacherAsync(int schoolId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.SchoolTeachers.Add(new SchoolTeacher { SchoolId = schoolId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveTeacherAsync(int schoolId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.SchoolTeachers
                .Where(x => x.SchoolId == schoolId && x.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
