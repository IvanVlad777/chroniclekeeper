using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ClimateDetailRepository : IClimateDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public ClimateDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClimateDetail> CreateAsync(ClimateDetail climateDetail, CancellationToken cancellationToken = default)
        {
            _context.ClimateDetails.Add(climateDetail);
            await _context.SaveChangesAsync(cancellationToken);
            return climateDetail;
        }

        public async Task<ClimateDetail?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ClimateDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<List<ClimateDetail>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.ClimateDetails.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(d => d.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<ClimateDetail> UpdateAsync(ClimateDetail climateDetail, CancellationToken cancellationToken = default)
        {
            _context.Entry(climateDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return climateDetail;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ClimateDetails
                .Where(d => d.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
