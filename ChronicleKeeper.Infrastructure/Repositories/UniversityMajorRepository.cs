using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class UniversityMajorRepository : IUniversityMajorRepository
    {
        private readonly ApplicationDbContext _context;

        public UniversityMajorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UniversityMajor> CreateAsync(UniversityMajor major, CancellationToken cancellationToken = default)
        {
            _context.UniversityMajors.Add(major);
            await _context.SaveChangesAsync(cancellationToken);
            return major;
        }

        public async Task<UniversityMajor?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.UniversityMajors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<UniversityMajor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.UniversityMajors
                .Include(m => m.University)
                .Include(m => m.Professors).ThenInclude(p => p.Character)
                .Include(m => m.Students).ThenInclude(st => st.Character)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<List<UniversityMajor>> GetAllAsync(int? worldId = null, int? universityId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.UniversityMajors.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(m => m.WorldId == wid);
            }
            if (universityId is int uid)
            {
                query = query.Where(m => m.UniversityId == uid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<UniversityMajor> UpdateAsync(UniversityMajor major, CancellationToken cancellationToken = default)
        {
            _context.Entry(major).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return major;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.UniversityMajors
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsProfessorLinkedAsync(int majorId, int characterId, CancellationToken cancellationToken = default)
        {
            return await _context.UniversityMajorProfessors
                .AnyAsync(x => x.UniversityMajorId == majorId && x.CharacterId == characterId, cancellationToken);
        }

        public async Task AddProfessorAsync(int majorId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.UniversityMajorProfessors.Add(new UniversityMajorProfessor { UniversityMajorId = majorId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveProfessorAsync(int majorId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.UniversityMajorProfessors
                .Where(x => x.UniversityMajorId == majorId && x.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsStudentLinkedAsync(int majorId, int characterId, CancellationToken cancellationToken = default)
        {
            return await _context.UniversityMajorStudents
                .AnyAsync(x => x.UniversityMajorId == majorId && x.CharacterId == characterId, cancellationToken);
        }

        public async Task AddStudentAsync(int majorId, int characterId, CancellationToken cancellationToken = default)
        {
            _context.UniversityMajorStudents.Add(new UniversityMajorStudent { UniversityMajorId = majorId, CharacterId = characterId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveStudentAsync(int majorId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.UniversityMajorStudents
                .Where(x => x.UniversityMajorId == majorId && x.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
