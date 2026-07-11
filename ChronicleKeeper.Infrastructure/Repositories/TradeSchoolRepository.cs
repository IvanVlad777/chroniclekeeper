using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class TradeSchoolRepository : ITradeSchoolRepository
    {
        private readonly ApplicationDbContext _context;

        public TradeSchoolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TradeSchool> CreateAsync(TradeSchool tradeSchool, CancellationToken cancellationToken = default)
        {
            _context.TradeSchools.Add(tradeSchool);
            await _context.SaveChangesAsync(cancellationToken);
            return tradeSchool;
        }

        public async Task<TradeSchool?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // _context.TradeSchools EF Core automatski filtrira na SchoolType = 'TradeSchool'.
            return await _context.TradeSchools
                .Include(t => t.Subjects)
                .Include(t => t.Alumni)
                .Include(t => t.TrainedProfessions).ThenInclude(pt => pt.Profession)
                .Include(t => t.Apprenticeships)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<TradeSchool?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.TradeSchools
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TradeSchool>> GetAllAsync(int? worldId = null, int? educationSystemId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.TradeSchools.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(t => t.WorldId == wid);
            }
            if (educationSystemId is int esid)
            {
                query = query.Where(t => t.EducationSystemId == esid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TradeSchool> UpdateAsync(TradeSchool tradeSchool, CancellationToken cancellationToken = default)
        {
            _context.Entry(tradeSchool).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return tradeSchool;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Subjects i TrainedProfessions kaskadiraju DB; Apprenticeships i EducationRecords
            // je handler već provjerio (Restrict).
            var deleted = await _context.TradeSchools
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountApprenticeshipsUsingTradeSchoolAsync(int tradeSchoolId, CancellationToken cancellationToken = default)
        {
            return await _context.Apprenticeships.CountAsync(a => a.TradeSchoolId == tradeSchoolId, cancellationToken);
        }

        public async Task<int> CountEducationRecordsUsingSchoolAsync(int schoolId, CancellationToken cancellationToken = default)
        {
            return await _context.EducationRecords.CountAsync(e => e.SchoolId == schoolId, cancellationToken);
        }
    }
}
