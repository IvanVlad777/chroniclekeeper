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
                .AsNoTracking()
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
    }
}
