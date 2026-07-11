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

            // 2c. Vjerska obrazovanja (Restrict→Character) i knjižnice (Restrict→University/Location)
            //     moraju nestati prije likova (korak 3) odn. lokacija (korak 6)
            await _context.ReligiousEducations
                .Where(re => re.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.Libraries
                .Where(l => l.WorldId == id)
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

            // 7bb. Diplomatski ugovori (Restrict FK na Nation s obje strane — moraju nestati prije nacija)
            await _context.DiplomaticAgreements
                .Where(a => a.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7c. Nacije (agreementi i likovi više nema pa Restrict ne blokira)
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

            // 7g. Političke stranke (Restrict FK na PoliticalIdeology i GovernmentSystem — moraju nestati prve)
            await _context.PoliticalParties
                .Where(p => p.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7h. Sustavi vlasti (Restrict FK na PoliticalIdeology — stranaka više nema pa Restrict ne blokira)
            await _context.GovernmentSystems
                .Where(g => g.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7i. Političke ideologije (stranaka i sustava vlasti više nema pa Restrict ne blokira)
            await _context.PoliticalIdeologies
                .Where(i => i.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7j. Pravni sustavi (samostalan entitet, bez ovisnosti unutar ovog bloka)
            await _context.LegalSystems
                .Where(l => l.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7k. Zanimanja (kaskadira JobRanks, Apprenticeships, Specialisations,
            //     ProfessionSapientSpecies/ProfessionSocialClass/ProfessionTradeSchool join tablice).
            //     Mora ići prije 7m: Apprenticeship.TradeSchoolId je Restrict, a ovim se
            //     korakom uklanjaju SVI Apprenticeship redovi (ProfessionId je required),
            //     pa TradeSchool poslije može nesmetano nestati.
            await _context.Professions
                .Where(p => p.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7l. Preostali obrazovni zapisi (Restrict→School/University; CharacterId-Cascade
            //     dio je već očišćen u koraku 3, ovo hvata institucijske zapise bez lika)
            await _context.EducationRecords
                .Where(e => e.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7m. Sustavi obrazovanja (kaskadira Schools/TradeSchools/Universities preko
            //     required EducationSystemId FK-a, što dalje kaskadira SchoolSubjects/UniversityMajors)
            await _context.EducationSystems
                .Where(e => e.WorldId == id)
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
