using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
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

            await _context.Creatures
                .Where(c => c.WorldId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.ParentCreatureId, (int?)null), cancellationToken);

            await _context.Corporations
                .Where(c => c.WorldId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.ParentCorporationId, (int?)null), cancellationToken);

            // RiverEcosystem.SourceLocationId/MouthLocationId su Restrict (dva FK-a na Locations sa
            // iste tablice ne smiju oba biti SetNull — SQL Server "multiple cascade paths"), pa se
            // moraju null-ati prije brisanja lokacija (korak 6).
            await _context.Set<RiverEcosystem>()
                .Where(r => r.WorldId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(r => r.SourceLocationId, (int?)null)
                    .SetProperty(r => r.MouthLocationId, (int?)null), cancellationToken);

            // OwnershipHistory.PreviousOwnerId/NewOwnerId su Restrict (dva FK-a na Character
            // sa iste tablice ne smiju oba biti SetNull — SQL Server "multiple cascade paths").
            // Moraju se null-ati prije brisanja likova (korak 3); redovi kasnije nestaju
            // kaskadno kad se obriše njihov Item (korak 7n).
            await _context.OwnershipHistories
                .Where(o => o.WorldId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(o => o.PreviousOwnerId, (int?)null)
                    .SetProperty(o => o.NewOwnerId, (int?)null), cancellationToken);

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

            // 7cc. Culture-detail entiteti (vlastiti WorldId; brišu se prije Kultura.
            // Cascade-djeca bi ionako nestala s Culture, ali Custom/CulturalInstitution imaju SetNull Culture FK
            // pa ostaju — zato eksplicitno brišemo svih 10 po WorldId. Join tablice cascade s child strane.)
            await _context.Customs.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.Clothing.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.ArtForms.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.Cuisines.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.Traditions.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.ArchitectureStyles.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.Folktales.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.Myths.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.CulturalInstitutions.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.CulturalFestivals.Where(x => x.WorldId == id).ExecuteDeleteAsync(cancellationToken);

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

            // 7i2. Cehovi (kaskadira GuildRanks + GuildFaction/GuildProfession/GuildSocialClass join
            //      tablice; Apprenticeship/EducationRecord.GuildId su SetNull). Moraju nestati prije
            //      LegalSystems (7j), EducationSystems (7m), TaxationSystems i Industries (Restrict FK-ovi).
            await _context.Guilds
                .Where(g => g.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7i3. Korporacije (self-ref ParentCorporationId već razvezan u koraku 1; kaskadira
            //      CorporateLeaderships + CorporationFaction/CorporationProfession join tablice;
            //      Apprenticeship.CorporationId je SetNull). Moraju nestati prije Industries,
            //      TaxationSystems i BankingSystems (Restrict FK-ovi).
            await _context.Corporations
                .Where(c => c.WorldId == id)
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

            // 7n. Predmeti (Restrict WorldId; SetNull FK-ovi na Character/Location/Faction ne
            //     ovise o redoslijedu — kaskadira OwnershipHistories preko Item.WorldId)
            await _context.Items
                .Where(i => i.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7o. Sposobnosti (kaskadira AbilityLevels i CharacterAbility join tablicu)
            await _context.Abilities
                .Where(a => a.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7p. References prema Content/Chapter/Episode su Restrict (izravni Contents->References
            //     Cascade zajedno s Contents->Chapters->References i Contents->Episodes->References
            //     bi stvorili TRI konvergentna cascade puta — SQL Server "multiple cascade paths").
            //     Moraju se ručno obrisati prije brisanja Contents (koji kaskadno briše Chapters/Episodes).
            await _context.References
                .Where(r => r.ContentId != null && r.Content!.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.References
                .Where(r => r.ChapterId != null && r.Chapter!.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.References
                .Where(r => r.EpisodeId != null && r.Episode!.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7q. Sadržaj (kaskadira Chapters/Episodes preko required BookId/SeriesId;
            //     References prema entitetima strane su Cascade pa nestaju automatski)
            await _context.Contents
                .Where(c => c.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7r. Povijesti (SetNull FK-ovi na Character/Location/Faction/Nation/Timeline ne
            //     ovise o redoslijedu)
            await _context.Histories
                .Where(h => h.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7s. Klimatske zone (kaskadira WeatherPatterns preko required ClimateZoneId FK-a,
            //     te ClimateZoneDetail/ClimateZoneSeason/LocationClimateZone join tablice)
            await _context.ClimateZones
                .Where(z => z.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7t. Klimatski detalji (samostalan entitet — join redovi već nestali u 7s)
            await _context.ClimateDetails
                .Where(d => d.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7u. Godišnja doba (samostalan entitet — join redovi već nestali u 7s)
            await _context.Seasons
                .Where(s => s.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7v. Stvorenja (self-ref ParentCreatureId već razvezan u koraku 1; History je SetNull;
            //     CreatureCity je Cascade s obje strane pa se čisti sam bez obzira na redoslijed)
            await _context.Creatures
                .Where(c => c.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7w. Ekonomski sustavi (Restrict FK-ovi na TaxationSystem/BankingSystem — moraju nestati
            //     prije njih; lokacije s EconomicSystemId Restrict FK-om već obrisane u koraku 6)
            await _context.EconomicSystems
                .Where(e => e.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7x. Porezni sustavi (cehovi/korporacije/ekonomski sustavi više nema pa Restrict ne blokira)
            await _context.TaxationSystems
                .Where(t => t.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7y. Bankarski sustavi (Restrict FK na Currency — moraju nestati prije valuta)
            await _context.BankingSystems
                .Where(b => b.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7z. Valute (bankarskih sustava više nema pa Restrict ne blokira)
            await _context.Currencies
                .Where(c => c.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7aa. Prirodni resursi (kaskadira NaturalResourceLocation/TradeRouteResource join tablice;
            //      Restrict FK na ExtractionMethod — moraju nestati prije metoda ekstrakcije)
            await _context.NaturalResources
                .Where(n => n.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7ab. Trgovačke rute (kaskadira TradeRouteLocation join tablicu; TradeRouteResource
            //      redovi već nestali u 7aa)
            await _context.TradeRoutes
                .Where(t => t.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7ac. Metode ekstrakcije (resursa više nema pa Restrict ne blokira)
            await _context.ExtractionMethods
                .Where(e => e.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7ad. Industrije (cehova i korporacija više nema pa Restrict ne blokira)
            await _context.Industries
                .Where(i => i.WorldId == id)
                .ExecuteDeleteAsync(cancellationToken);

            // 7ae. Vojska — leaf-first (nema Restrict FK-ova u klasteru; join tablice
            // ArmyBattle/MilitaryUnitEquipment/MilitaryOrganizationCountry/-Faction kaskadiraju).
            // Ranks -> Units -> Armies -> Battles -> Equipment -> Organizations -> Doctrines.
            await _context.MilitaryRanks.Where(r => r.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.MilitaryUnits.Where(u => u.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.Armies.Where(a => a.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.Battles.Where(b => b.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.MilitaryEquipments.Where(e => e.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.MilitaryOrganizations.Where(o => o.WorldId == id).ExecuteDeleteAsync(cancellationToken);
            await _context.MilitaryDoctrines.Where(d => d.WorldId == id).ExecuteDeleteAsync(cancellationToken);

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
