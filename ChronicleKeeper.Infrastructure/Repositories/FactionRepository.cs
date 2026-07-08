using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class FactionRepository : IFactionRepository
    {
        private readonly ApplicationDbContext _context;

        public FactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Faction> CreateAsync(Faction faction, CancellationToken cancellationToken = default)
        {
            _context.Factions.Add(faction);
            await _context.SaveChangesAsync(cancellationToken);
            return faction;
        }

        public async Task<Faction?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Factions
                .Include(f => f.Leader)
                .Include(f => f.Headquarters)
                .Include(f => f.Members).ThenInclude(m => m.Character)
                .Include(f => f.Tags).ThenInclude(t => t.Tag)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public async Task<Faction?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Factions
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public async Task<List<Faction>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Factions.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(f => f.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Faction> UpdateAsync(Faction faction, CancellationToken cancellationToken = default)
        {
            _context.Entry(faction).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return faction;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // DB kaskadira FactionMembers i FactionTags
            var deleted = await _context.Factions
                .Where(f => f.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<FactionMember> AddMemberAsync(FactionMember member, CancellationToken cancellationToken = default)
        {
            _context.FactionMembers.Add(member);
            await _context.SaveChangesAsync(cancellationToken);

            // Učitaj Character za mapiranje CharacterName u DTO
            await _context.Entry(member).Reference(m => m.Character).LoadAsync(cancellationToken);
            return member;
        }

        public async Task<bool> RemoveMemberAsync(int factionId, int characterId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.FactionMembers
                .Where(m => m.FactionId == factionId && m.CharacterId == characterId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsMemberAsync(int factionId, int characterId, CancellationToken cancellationToken = default)
        {
            return await _context.FactionMembers
                .AnyAsync(m => m.FactionId == factionId && m.CharacterId == characterId, cancellationToken);
        }
    }
}
