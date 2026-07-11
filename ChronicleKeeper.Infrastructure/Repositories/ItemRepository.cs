using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Item> CreateAsync(Item item, CancellationToken cancellationToken = default)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return item;
        }

        public async Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Items
                .Include(i => i.CurrentOwner)
                .Include(i => i.StoredAt)
                .Include(i => i.Faction)
                .Include(i => i.OwnershipHistory)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<Item?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<List<Item>> GetAllAsync(int? worldId = null, int? currentOwnerId = null, int? factionId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Items.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(i => i.WorldId == wid);
            }
            if (currentOwnerId is int ownerId)
            {
                query = query.Where(i => i.CurrentOwnerId == ownerId);
            }
            if (factionId is int fid)
            {
                query = query.Where(i => i.FactionId == fid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Item> UpdateAsync(Item item, CancellationToken cancellationToken = default)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return item;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Items
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
