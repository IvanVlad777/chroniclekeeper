using ChronicleKeeper.Core.DTOs.History;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public HistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<History> CreateAsync(History history, CancellationToken cancellationToken = default)
        {
            _context.Histories.Add(history);
            await _context.SaveChangesAsync(cancellationToken);
            return history;
        }

        public async Task<History?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Histories
                .Include(h => h.Timelines)
                    .ThenInclude(t => t.Events)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<HistoryLinkDto>> GetLinkedEntitiesAsync(int historyId, CancellationToken cancellationToken = default)
        {
            // History has no navigation back to its owners — gather them per type.
            // Covers the four "major vertical" types shown on the hub; more can be
            // added here as other entities start surfacing their History link in the UI.
            var links = new List<HistoryLinkDto>();

            links.AddRange(await _context.Characters.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Character", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Locations.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Location", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Factions.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Faction", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Nations.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Nation", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.ClimateZones.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "ClimateZone", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.ClimateDetails.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "ClimateDetail", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Seasons.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Season", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Creatures.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Creature", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.EconomicSystems.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "EconomicSystem", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Currencies.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Currency", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.BankingSystems.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "BankingSystem", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.TaxationSystems.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "TaxationSystem", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.TradeRoutes.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "TradeRoute", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.NaturalResources.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "NaturalResource", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.ExtractionMethods.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "ExtractionMethod", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Industries.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Industry", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Guilds.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Guild", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Corporations.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Corporation", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.MilitaryDoctrines.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "MilitaryDoctrine", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.MilitaryOrganizations.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "MilitaryOrganization", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Armies.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Army", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.MilitaryUnits.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "MilitaryUnit", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Battles.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Battle", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.MilitaryEquipments.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "MilitaryEquipment", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.Deities.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "Deity", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.HolySites.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "HolySite", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.ReligiousTexts.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "ReligiousText", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.ReligiousOrders.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "ReligiousOrder", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));
            links.AddRange(await _context.ReligiousFestivals.Where(x => x.HistoryId == historyId)
                .Select(x => new HistoryLinkDto { Type = "ReligiousFestival", Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken));

            return links;
        }

        public async Task<bool> TargetExistsInWorldAsync(HistoryLinkTargetType targetType, int targetId, int worldId, CancellationToken cancellationToken = default)
        {
            return targetType switch
            {
                HistoryLinkTargetType.Character => await _context.Characters.AnyAsync(c => c.Id == targetId && c.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Location => await _context.Locations.AnyAsync(l => l.Id == targetId && l.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Faction => await _context.Factions.AnyAsync(f => f.Id == targetId && f.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Nation => await _context.Nations.AnyAsync(n => n.Id == targetId && n.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.ClimateZone => await _context.ClimateZones.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.ClimateDetail => await _context.ClimateDetails.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Season => await _context.Seasons.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Creature => await _context.Creatures.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.EconomicSystem => await _context.EconomicSystems.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Currency => await _context.Currencies.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.BankingSystem => await _context.BankingSystems.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.TaxationSystem => await _context.TaxationSystems.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.TradeRoute => await _context.TradeRoutes.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.NaturalResource => await _context.NaturalResources.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.ExtractionMethod => await _context.ExtractionMethods.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Industry => await _context.Industries.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Guild => await _context.Guilds.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Corporation => await _context.Corporations.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.MilitaryDoctrine => await _context.MilitaryDoctrines.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.MilitaryOrganization => await _context.MilitaryOrganizations.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Army => await _context.Armies.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.MilitaryUnit => await _context.MilitaryUnits.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Battle => await _context.Battles.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.MilitaryEquipment => await _context.MilitaryEquipments.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.Deity => await _context.Deities.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.HolySite => await _context.HolySites.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.ReligiousText => await _context.ReligiousTexts.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.ReligiousOrder => await _context.ReligiousOrders.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                HistoryLinkTargetType.ReligiousFestival => await _context.ReligiousFestivals.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                _ => false,
            };
        }

        public async Task<int> SetTargetHistoryAsync(HistoryLinkTargetType targetType, int targetId, int? historyId, int? onlyIfCurrentHistoryId = null, CancellationToken cancellationToken = default)
        {
            // ExecuteUpdate keeps this a single UPDATE; the optional guard makes unlink
            // no-op unless the target currently points at the expected history.
            return targetType switch
            {
                HistoryLinkTargetType.Character => await _context.Characters
                    .Where(c => c.Id == targetId && (onlyIfCurrentHistoryId == null || c.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Location => await _context.Locations
                    .Where(l => l.Id == targetId && (onlyIfCurrentHistoryId == null || l.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(l => l.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Faction => await _context.Factions
                    .Where(f => f.Id == targetId && (onlyIfCurrentHistoryId == null || f.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(f => f.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Nation => await _context.Nations
                    .Where(n => n.Id == targetId && (onlyIfCurrentHistoryId == null || n.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(n => n.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.ClimateZone => await _context.ClimateZones
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.ClimateDetail => await _context.ClimateDetails
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Season => await _context.Seasons
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Creature => await _context.Creatures
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.EconomicSystem => await _context.EconomicSystems
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Currency => await _context.Currencies
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.BankingSystem => await _context.BankingSystems
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.TaxationSystem => await _context.TaxationSystems
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.TradeRoute => await _context.TradeRoutes
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.NaturalResource => await _context.NaturalResources
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.ExtractionMethod => await _context.ExtractionMethods
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Industry => await _context.Industries
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Guild => await _context.Guilds
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Corporation => await _context.Corporations
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.MilitaryDoctrine => await _context.MilitaryDoctrines
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.MilitaryOrganization => await _context.MilitaryOrganizations
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Army => await _context.Armies
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.MilitaryUnit => await _context.MilitaryUnits
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Battle => await _context.Battles
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.MilitaryEquipment => await _context.MilitaryEquipments
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.Deity => await _context.Deities
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.HolySite => await _context.HolySites
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.ReligiousText => await _context.ReligiousTexts
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.ReligiousOrder => await _context.ReligiousOrders
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                HistoryLinkTargetType.ReligiousFestival => await _context.ReligiousFestivals
                    .Where(x => x.Id == targetId && (onlyIfCurrentHistoryId == null || x.HistoryId == onlyIfCurrentHistoryId))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.HistoryId, historyId), cancellationToken),
                _ => 0,
            };
        }

        public async Task<History?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Histories
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<History>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Histories.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(h => h.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<History> UpdateAsync(History history, CancellationToken cancellationToken = default)
        {
            _context.Entry(history).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return history;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Histories
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
