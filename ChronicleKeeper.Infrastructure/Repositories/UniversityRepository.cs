using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly ApplicationDbContext _context;

        public UniversityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<University> CreateAsync(University university, CancellationToken cancellationToken = default)
        {
            _context.Universities.Add(university);
            await _context.SaveChangesAsync(cancellationToken);
            return university;
        }

        public async Task<University?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Universities
                .Include(u => u.Majors)
                .Include(u => u.Alumni)
                .Include(u => u.Students).ThenInclude(us => us.Character)
                .Include(u => u.Professors).ThenInclude(up => up.Character)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<University?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Universities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<List<University>> GetAllAsync(int? worldId = null, int? educationSystemId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Universities.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(u => u.WorldId == wid);
            }
            if (educationSystemId is int esid)
            {
                query = query.Where(u => u.EducationSystemId == esid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<University> UpdateAsync(University university, CancellationToken cancellationToken = default)
        {
            _context.Entry(university).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return university;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Majors kaskadira DB; EducationRecords/Libraries je handler već provjerio (Restrict)
            var deleted = await _context.Universities
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> ExistsInWorldAsync(int universityId, int worldId, CancellationToken cancellationToken = default)
        {
            return await _context.Universities
                .AsNoTracking()
                .AnyAsync(u => u.Id == universityId && u.WorldId == worldId, cancellationToken);
        }

        public async Task<int> CountEducationRecordsUsingUniversityAsync(int universityId, CancellationToken cancellationToken = default)
        {
            return await _context.EducationRecords.CountAsync(e => e.UniversityId == universityId, cancellationToken);
        }

        public async Task<int> CountLibrariesUsingUniversityAsync(int universityId, CancellationToken cancellationToken = default)
        {
            return await _context.Libraries.CountAsync(l => l.UniversityId == universityId, cancellationToken);
        }

        public Task<bool> IsStudentLinkedAsync(int universityId, int characterId, CancellationToken cancellationToken = default)
            => _context.UniversityStudents.AnyAsync(x => x.UniversityId == universityId && x.CharacterId == characterId, cancellationToken);

        public async Task AddStudentAsync(int universityId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.UniversityStudents.Add(new UniversityStudent { UniversityId = universityId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveStudentAsync(int universityId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.UniversityStudents
                .Where(x => x.UniversityId == universityId && x.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public Task<bool> IsProfessorLinkedAsync(int universityId, int characterId, CancellationToken cancellationToken = default)
            => _context.UniversityProfessors.AnyAsync(x => x.UniversityId == universityId && x.CharacterId == characterId, cancellationToken);

        public async Task AddProfessorAsync(int universityId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.UniversityProfessors.Add(new UniversityProfessor { UniversityId = universityId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveProfessorAsync(int universityId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.UniversityProfessors
                .Where(x => x.UniversityId == universityId && x.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
