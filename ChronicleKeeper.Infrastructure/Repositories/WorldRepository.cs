using ChronicleKeeper.Core.Entities.Worlds;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class WorldRepository : IWorldRepository
    {
        private readonly ApplicationDbContext _context;

        public WorldRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<World> CreateAsync(World world, CancellationToken cancellationToken = default)
        {
            _context.Worlds.Add(world);
            await _context.SaveChangesAsync(cancellationToken);
            return world;
        }

        public async Task<World?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Worlds
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<List<World>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Worlds
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<World>> GetByOwnerAsync(string ownerId, CancellationToken cancellationToken = default)
        {
            return await _context.Worlds
                .AsNoTracking()
                .Where(w => w.OwnerId == ownerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<World> UpdateAsync(World world, CancellationToken cancellationToken = default)
        {
            _context.Worlds.Update(world);
            await _context.SaveChangesAsync(cancellationToken);
            return world;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Worlds.AnyAsync(w => w.Id == id, cancellationToken);
        }

        /// <summary>
        /// Briše svijet i SVE njegove podatke. WorldId FK-ovi su namjerno Restrict
        /// (cascade od Worlda bi stvorio višestruke cascade putanje na SQL Serveru),
        /// pa se retci brišu ručno, u redoslijedu koji poštuje preostale FK-ove.
        /// </summary>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            // 1. Razveži self-referencirajuće FK-ove (Restrict) prije brisanja
            await _context.Characters
                .Where(c => c.WorldId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.FatherId, (int?)null)
                    .SetProperty(c => c.MotherId, (int?)null), cancellationToken);

            await _context.Locations
                .Where(l => l.WorldId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.ParentLocationId, (int?)null), cancellationToken);

            // 2. Veze među likovima — po OBJE strane (RelatedCharacterId je Restrict;
            //    red čiji je vlasnik izvan svijeta inače bi blokirao brisanje likova)
            await _context.CharacterRelationships
                .Where(r => r.Character!.WorldId == id || r.RelatedCharacter!.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 3. Likovi (DB kaskadira FactionMembers + CharacterTags; SET NULL na Faction.LeaderId)
            await _context.Characters
                .Where(c => c.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 4. Timelines (kaskadira TimelineEvents)
            await _context.Timelines
                .Where(t => t.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);
            // Sigurnosna mreža za eventualne evente čiji WorldId ne odgovara
            // svijetu njihovog Timelinea (na konzistentnim podacima briše 0 redova)
            await _context.TimelineEvents
                .Where(e => e.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 5. Frakcije (kaskadira FactionTags)
            await _context.Factions
                .Where(f => f.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 6. Lokacije (kaskadira LocationTags)
            await _context.Locations
                .Where(l => l.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7. Vrste (kaskadira Races — likova više nema pa Restrict ne blokira)
            await _context.SapientSpecies
                .Where(s => s.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7b. Društveni slojevi (likova više nema pa Restrict ne blokira)
            await _context.SocialClasses
                .Where(sc => sc.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7c. Nacije (likova više nema pa Restrict ne blokira)
            await _context.Nations
                .Where(n => n.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7d. Kulture (moraju nestati prije Religions/Languages — Culture ima Restrict FK na oboje)
            await _context.Cultures
                .Where(c => c.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7e. Religije (likova i kultura više nema pa Restrict ne blokira)
            await _context.Religions
                .Where(r => r.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7f. Jezici (kultura više nema pa Restrict ne blokira)
            await _context.Languages
                .Where(l => l.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 8. Tagovi i bilješke
            await _context.Tags
                .Where(t => t.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.Notes
                .Where(n => n.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 9. Sam svijet
            await _context.Worlds
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
    }
}
