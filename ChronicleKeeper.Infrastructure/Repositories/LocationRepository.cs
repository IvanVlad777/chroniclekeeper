using ChronicleKeeper.Core.DTOs.Location;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Location> CreateAsync(Location location, CancellationToken cancellationToken = default)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync(cancellationToken);
            return location;
        }

        public async Task<Location?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // _context.Locations (base DbSet) returns any TPH subtype (Continent/Region/Country/City/District) — namjerno.
            return await _context.Locations
                .Include(l => l.ParentLocation)
                .Include(l => l.SubLocations)
                .Include(l => l.Tags).ThenInclude(t => t.Tag)
                .Include(l => l.History)
                .Include(l => l.Schools)
                .Include(l => ((Country)l).GovernmentSystem)
                .Include(l => ((Country)l).LegalSystem)
                .Include(l => ((Country)l).EducationSystem)
                .Include(l => ((Country)l).EconomicSystem)
                .Include(l => ((City)l).GovernmentSystem)
                .Include(l => ((City)l).LegalSystem)
                .Include(l => ((City)l).EducationSystem)
                .Include(l => ((City)l).EconomicSystem)
                .Include(l => ((Region)l).OriginOfSapientSpecies).ThenInclude(rs => rs.SapientSpecies)
                // Country cross-links (+ reverse read-only military organizations)
                .Include(l => ((Country)l).Industries).ThenInclude(x => x.Industry)
                .Include(l => ((Country)l).Corporations).ThenInclude(x => x.Corporation)
                .Include(l => ((Country)l).Guilds).ThenInclude(x => x.Guild)
                .Include(l => ((Country)l).PoliticalParties).ThenInclude(x => x.PoliticalParty)
                .Include(l => ((Country)l).Nations).ThenInclude(x => x.Nation)
                .Include(l => ((Country)l).Factions).ThenInclude(x => x.Faction)
                .Include(l => ((Country)l).PredominantCultures).ThenInclude(x => x.Culture)
                .Include(l => ((Country)l).Religions).ThenInclude(x => x.Religion)
                .Include(l => ((Country)l).MilitaryOrganizations).ThenInclude(x => x.MilitaryOrganization)
                // City cross-links (+ reverse read-only inhabiting creatures)
                .Include(l => ((City)l).Industries).ThenInclude(x => x.Industry)
                .Include(l => ((City)l).Corporations).ThenInclude(x => x.Corporation)
                .Include(l => ((City)l).Guilds).ThenInclude(x => x.Guild)
                .Include(l => ((City)l).CulturalInstitutions).ThenInclude(x => x.CulturalInstitution)
                .Include(l => ((City)l).PoliticalParties).ThenInclude(x => x.PoliticalParty)
                .Include(l => ((City)l).PredominantCultures).ThenInclude(x => x.Culture)
                .Include(l => ((City)l).Nations).ThenInclude(x => x.Nation)
                .Include(l => ((City)l).Religions).ThenInclude(x => x.Religion)
                .Include(l => ((City)l).InhabitingCreatures).ThenInclude(x => x.Creature)
                // Reverse read-only trade routes (owned by TradeRoute via TradeRouteLocation) — base Location nav
                .Include(l => l.TradeRouteLinks).ThenInclude(x => x.TradeRoute)
                // Reverse read-only timeline events that took place at this location
                .Include(l => l.TimelineEvents)
                .Include(l => ((RiverEcosystem)l).SourceLocation)
                .Include(l => ((RiverEcosystem)l).MouthLocation)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<Location?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<List<Location>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Locations.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(l => l.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Location> UpdateAsync(Location location, CancellationToken cancellationToken = default)
        {
            _context.Entry(location).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return location;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // LocationTags kaskadira DB; Faction.HeadquartersId je SET NULL;
            // djeca su provjerena u handleru (Restrict)
            var deleted = await _context.Locations
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> ExistsInWorldAsync(int locationId, int worldId, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .AnyAsync(l => l.Id == locationId && l.WorldId == worldId, cancellationToken);
        }

        public async Task<bool> HasChildrenAsync(int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .AnyAsync(l => l.ParentLocationId == locationId, cancellationToken);
        }

        public async Task<bool> IsReferencedAsRiverEndpointAsync(int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RiverEcosystem>()
                .AnyAsync(r => r.SourceLocationId == locationId || r.MouthLocationId == locationId, cancellationToken);
        }

        public async Task<bool> IsReferencedByHolySiteAsync(int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<HolySite>()
                .AnyAsync(h => h.LocationId == locationId, cancellationToken);
        }

        public async Task<bool> WouldCreateCycleAsync(int locationId, int newParentId, CancellationToken cancellationToken = default)
        {
            // Hodaj uzlazno od novog roditelja; ako lanac dođe do same lokacije — ciklus
            int? current = newParentId;
            var visited = new HashSet<int>();
            while (current is int cid)
            {
                if (cid == locationId) return true;
                if (!visited.Add(cid)) return true; // postojeći ciklus u podacima — ne pogoršavaj

                current = await _context.Locations
                    .Where(l => l.Id == cid)
                    .Select(l => l.ParentLocationId)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            return false;
        }

        public async Task<bool> IsNativeSpeciesLinkedAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            return await _context.RegionSapientSpecies
                .AnyAsync(rs => rs.RegionId == regionId && rs.SapientSpeciesId == sapientSpeciesId, cancellationToken);
        }

        public async Task AddNativeSpeciesAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            _context.RegionSapientSpecies.Add(new RegionSapientSpecies { RegionId = regionId, SapientSpeciesId = sapientSpeciesId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveNativeSpeciesAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.RegionSapientSpecies
                .Where(rs => rs.RegionId == regionId && rs.SapientSpeciesId == sapientSpeciesId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        // ---- Country/City ↔ X cross-links ----

        public Task<bool> CrossLinkTargetExistsInWorldAsync(LocationLinkTargetType targetType, int targetId, int worldId, CancellationToken cancellationToken = default)
        {
            return targetType switch
            {
                LocationLinkTargetType.Industry => _context.Industries.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.Corporation => _context.Corporations.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.Guild => _context.Guilds.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.PoliticalParty => _context.PoliticalParties.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.Nation => _context.Nations.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.Faction => _context.Factions.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.Culture => _context.Cultures.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.Religion => _context.Religions.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                LocationLinkTargetType.CulturalInstitution => _context.CulturalInstitutions.AnyAsync(x => x.Id == targetId && x.WorldId == worldId, cancellationToken),
                _ => Task.FromResult(false),
            };
        }

        public Task<bool> IsCrossLinkedAsync(int locationId, bool isCity, LocationLinkTargetType targetType, int targetId, CancellationToken cancellationToken = default)
        {
            return (isCity, targetType) switch
            {
                (false, LocationLinkTargetType.Industry) => _context.CountryIndustries.AnyAsync(x => x.CountryId == locationId && x.IndustryId == targetId, cancellationToken),
                (false, LocationLinkTargetType.Corporation) => _context.CountryCorporations.AnyAsync(x => x.CountryId == locationId && x.CorporationId == targetId, cancellationToken),
                (false, LocationLinkTargetType.Guild) => _context.CountryGuilds.AnyAsync(x => x.CountryId == locationId && x.GuildId == targetId, cancellationToken),
                (false, LocationLinkTargetType.PoliticalParty) => _context.CountryPoliticalParties.AnyAsync(x => x.CountryId == locationId && x.PoliticalPartyId == targetId, cancellationToken),
                (false, LocationLinkTargetType.Nation) => _context.CountryNations.AnyAsync(x => x.CountryId == locationId && x.NationId == targetId, cancellationToken),
                (false, LocationLinkTargetType.Faction) => _context.CountryFactions.AnyAsync(x => x.CountryId == locationId && x.FactionId == targetId, cancellationToken),
                (false, LocationLinkTargetType.Culture) => _context.CountryCultures.AnyAsync(x => x.CountryId == locationId && x.CultureId == targetId, cancellationToken),
                (false, LocationLinkTargetType.Religion) => _context.CountryReligions.AnyAsync(x => x.CountryId == locationId && x.ReligionId == targetId, cancellationToken),
                (true, LocationLinkTargetType.Industry) => _context.CityIndustries.AnyAsync(x => x.CityId == locationId && x.IndustryId == targetId, cancellationToken),
                (true, LocationLinkTargetType.Corporation) => _context.CityCorporations.AnyAsync(x => x.CityId == locationId && x.CorporationId == targetId, cancellationToken),
                (true, LocationLinkTargetType.Guild) => _context.CityGuilds.AnyAsync(x => x.CityId == locationId && x.GuildId == targetId, cancellationToken),
                (true, LocationLinkTargetType.CulturalInstitution) => _context.CityCulturalInstitutions.AnyAsync(x => x.CityId == locationId && x.CulturalInstitutionId == targetId, cancellationToken),
                (true, LocationLinkTargetType.PoliticalParty) => _context.CityPoliticalParties.AnyAsync(x => x.CityId == locationId && x.PoliticalPartyId == targetId, cancellationToken),
                (true, LocationLinkTargetType.Culture) => _context.CityCultures.AnyAsync(x => x.CityId == locationId && x.CultureId == targetId, cancellationToken),
                (true, LocationLinkTargetType.Nation) => _context.CityNations.AnyAsync(x => x.CityId == locationId && x.NationId == targetId, cancellationToken),
                (true, LocationLinkTargetType.Religion) => _context.CityReligions.AnyAsync(x => x.CityId == locationId && x.ReligionId == targetId, cancellationToken),
                _ => Task.FromResult(false),
            };
        }

        public async Task AddCrossLinkAsync(int locationId, bool isCity, LocationLinkTargetType targetType, int targetId, CancellationToken cancellationToken = default)
        {
            object join = (isCity, targetType) switch
            {
                (false, LocationLinkTargetType.Industry) => new CountryIndustry { CountryId = locationId, IndustryId = targetId },
                (false, LocationLinkTargetType.Corporation) => new CountryCorporation { CountryId = locationId, CorporationId = targetId },
                (false, LocationLinkTargetType.Guild) => new CountryGuild { CountryId = locationId, GuildId = targetId },
                (false, LocationLinkTargetType.PoliticalParty) => new CountryPoliticalParty { CountryId = locationId, PoliticalPartyId = targetId },
                (false, LocationLinkTargetType.Nation) => new CountryNation { CountryId = locationId, NationId = targetId },
                (false, LocationLinkTargetType.Faction) => new CountryFaction { CountryId = locationId, FactionId = targetId },
                (false, LocationLinkTargetType.Culture) => new CountryCulture { CountryId = locationId, CultureId = targetId },
                (false, LocationLinkTargetType.Religion) => new CountryReligion { CountryId = locationId, ReligionId = targetId },
                (true, LocationLinkTargetType.Industry) => new CityIndustry { CityId = locationId, IndustryId = targetId },
                (true, LocationLinkTargetType.Corporation) => new CityCorporation { CityId = locationId, CorporationId = targetId },
                (true, LocationLinkTargetType.Guild) => new CityGuild { CityId = locationId, GuildId = targetId },
                (true, LocationLinkTargetType.CulturalInstitution) => new CityCulturalInstitution { CityId = locationId, CulturalInstitutionId = targetId },
                (true, LocationLinkTargetType.PoliticalParty) => new CityPoliticalParty { CityId = locationId, PoliticalPartyId = targetId },
                (true, LocationLinkTargetType.Culture) => new CityCulture { CityId = locationId, CultureId = targetId },
                (true, LocationLinkTargetType.Nation) => new CityNation { CityId = locationId, NationId = targetId },
                (true, LocationLinkTargetType.Religion) => new CityReligion { CityId = locationId, ReligionId = targetId },
                _ => throw new InvalidOperationException($"Unsupported cross-link {targetType} for {(isCity ? "City" : "Country")}."),
            };
            _context.Add(join);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveCrossLinkAsync(int locationId, bool isCity, LocationLinkTargetType targetType, int targetId, CancellationToken cancellationToken = default)
        {
            var deleted = (isCity, targetType) switch
            {
                (false, LocationLinkTargetType.Industry) => await _context.CountryIndustries.Where(x => x.CountryId == locationId && x.IndustryId == targetId).ExecuteDeleteAsync(cancellationToken),
                (false, LocationLinkTargetType.Corporation) => await _context.CountryCorporations.Where(x => x.CountryId == locationId && x.CorporationId == targetId).ExecuteDeleteAsync(cancellationToken),
                (false, LocationLinkTargetType.Guild) => await _context.CountryGuilds.Where(x => x.CountryId == locationId && x.GuildId == targetId).ExecuteDeleteAsync(cancellationToken),
                (false, LocationLinkTargetType.PoliticalParty) => await _context.CountryPoliticalParties.Where(x => x.CountryId == locationId && x.PoliticalPartyId == targetId).ExecuteDeleteAsync(cancellationToken),
                (false, LocationLinkTargetType.Nation) => await _context.CountryNations.Where(x => x.CountryId == locationId && x.NationId == targetId).ExecuteDeleteAsync(cancellationToken),
                (false, LocationLinkTargetType.Faction) => await _context.CountryFactions.Where(x => x.CountryId == locationId && x.FactionId == targetId).ExecuteDeleteAsync(cancellationToken),
                (false, LocationLinkTargetType.Culture) => await _context.CountryCultures.Where(x => x.CountryId == locationId && x.CultureId == targetId).ExecuteDeleteAsync(cancellationToken),
                (false, LocationLinkTargetType.Religion) => await _context.CountryReligions.Where(x => x.CountryId == locationId && x.ReligionId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.Industry) => await _context.CityIndustries.Where(x => x.CityId == locationId && x.IndustryId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.Corporation) => await _context.CityCorporations.Where(x => x.CityId == locationId && x.CorporationId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.Guild) => await _context.CityGuilds.Where(x => x.CityId == locationId && x.GuildId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.CulturalInstitution) => await _context.CityCulturalInstitutions.Where(x => x.CityId == locationId && x.CulturalInstitutionId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.PoliticalParty) => await _context.CityPoliticalParties.Where(x => x.CityId == locationId && x.PoliticalPartyId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.Culture) => await _context.CityCultures.Where(x => x.CityId == locationId && x.CultureId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.Nation) => await _context.CityNations.Where(x => x.CityId == locationId && x.NationId == targetId).ExecuteDeleteAsync(cancellationToken),
                (true, LocationLinkTargetType.Religion) => await _context.CityReligions.Where(x => x.CityId == locationId && x.ReligionId == targetId).ExecuteDeleteAsync(cancellationToken),
                _ => 0,
            };
            return deleted > 0;
        }
    }
}
