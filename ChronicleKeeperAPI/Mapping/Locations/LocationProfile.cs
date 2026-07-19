using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Location;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using static ChronicleKeeper.Core.Enums.EcosystemEnums;

/// <summary>
/// Location is a TPH root (Continent/Region/Country/City/District, plus the 6 plain types that
/// stay a bare Location). Each ForMember below delegates to a static helper that pattern-matches
/// on the runtime subtype to pick the right field(s), leaving the rest null — mirrors ContentProfile.
/// </summary>
public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, LocationDto>()
            .ForMember(d => d.ContinentSpecifics, opt => opt.MapFrom(src => GetContinentSpecifics(src)))
            .ForMember(d => d.RegionSpecifics, opt => opt.MapFrom(src => GetRegionSpecifics(src)))
            .ForMember(d => d.GovernmentSystemId, opt => opt.MapFrom(src => GetGovernmentSystemId(src)))
            .ForMember(d => d.LegalSystemId, opt => opt.MapFrom(src => GetLegalSystemId(src)))
            .ForMember(d => d.EducationSystemId, opt => opt.MapFrom(src => GetEducationSystemId(src)))
            .ForMember(d => d.EconomicSystemId, opt => opt.MapFrom(src => GetEconomicSystemId(src)))
            .ForMember(d => d.IsCapital, opt => opt.MapFrom(src => GetIsCapital(src)))
            .ForMember(d => d.DistrictType, opt => opt.MapFrom(src => GetDistrictType(src)))
            .ForMember(d => d.LandmarkType, opt => opt.MapFrom(src => GetLandmarkType(src)))
            .ForMember(d => d.UniqueFeatures, opt => opt.MapFrom(src => GetUniqueFeatures(src)))
            .ForMember(d => d.WaterDepth, opt => opt.MapFrom(src => GetWaterDepth(src)))
            .ForMember(d => d.Volume, opt => opt.MapFrom(src => GetVolume(src)))
            .ForMember(d => d.MaxDepth, opt => opt.MapFrom(src => GetMaxDepth(src)))
            .ForMember(d => d.IsFreshwater, opt => opt.MapFrom(src => GetIsFreshwater(src)))
            .ForMember(d => d.RiverLength, opt => opt.MapFrom(src => GetRiverLength(src)))
            .ForMember(d => d.SourceLocationId, opt => opt.MapFrom(src => GetSourceLocationId(src)))
            .ForMember(d => d.MouthLocationId, opt => opt.MapFrom(src => GetMouthLocationId(src)))
            .ForMember(d => d.MaxElevation, opt => opt.MapFrom(src => GetMaxElevation(src)))
            .ForMember(d => d.Prominence, opt => opt.MapFrom(src => GetProminence(src)))
            .ForMember(d => d.MountainRangeLength, opt => opt.MapFrom(src => GetMountainRangeLength(src)))
            .ForMember(d => d.IsSaltwater, opt => opt.MapFrom(src => GetIsSaltwater(src)))
            .ForMember(d => d.DesertKind, opt => opt.MapFrom(src => GetDesertKind(src)))
            .ForMember(d => d.ForestKind, opt => opt.MapFrom(src => GetForestKind(src)))
            .ForMember(d => d.CaveDepth, opt => opt.MapFrom(src => GetCaveDepth(src)))
            .ForMember(d => d.CaveKind, opt => opt.MapFrom(src => GetCaveKind(src)))
            .ForMember(d => d.GrasslandKind, opt => opt.MapFrom(src => GetGrasslandKind(src)));

        CreateMap<Location, LocationDetailsDto>()
            .IncludeBase<Location, LocationDto>()
            .ForMember(dest => dest.ParentLocation, opt => opt.MapFrom(src => src.ParentLocation == null
                ? null
                : new ReferenceDto { Id = src.ParentLocation.Id, Name = src.ParentLocation.Name }))
            .ForMember(dest => dest.SubLocations, opt => opt.MapFrom(src => src.SubLocations
                .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags
                .Where(t => t.Tag != null)
                .Select(t => new ReferenceDto { Id = t.Tag!.Id, Name = t.Tag.Name })))
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null
                ? null
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
            .ForMember(dest => dest.GovernmentSystem, opt => opt.MapFrom(src => GetGovernmentSystem(src)))
            .ForMember(dest => dest.LegalSystem, opt => opt.MapFrom(src => GetLegalSystem(src)))
            .ForMember(dest => dest.EducationSystem, opt => opt.MapFrom(src => GetEducationSystem(src)))
            .ForMember(dest => dest.EconomicSystem, opt => opt.MapFrom(src => GetEconomicSystem(src)))
            .ForMember(dest => dest.Schools, opt => opt.MapFrom(src => src.Schools
                .Select(s => new ReferenceDto { Id = s.Id, Name = s.Name })))
            .ForMember(dest => dest.NativeSpecies, opt => opt.MapFrom(src => GetNativeSpecies(src)))
            .ForMember(dest => dest.SourceLocation, opt => opt.MapFrom(src => GetSourceLocation(src)))
            .ForMember(dest => dest.MouthLocation, opt => opt.MapFrom(src => GetMouthLocation(src)))
            // Country/City cross-links (editable)
            .ForMember(dest => dest.Industries, opt => opt.MapFrom(src => GetIndustries(src)))
            .ForMember(dest => dest.Corporations, opt => opt.MapFrom(src => GetCorporations(src)))
            .ForMember(dest => dest.Guilds, opt => opt.MapFrom(src => GetGuilds(src)))
            .ForMember(dest => dest.PoliticalParties, opt => opt.MapFrom(src => GetPoliticalParties(src)))
            .ForMember(dest => dest.Nations, opt => opt.MapFrom(src => GetNations(src)))
            .ForMember(dest => dest.Cultures, opt => opt.MapFrom(src => GetCultures(src)))
            .ForMember(dest => dest.Religions, opt => opt.MapFrom(src => GetReligions(src)))
            .ForMember(dest => dest.Factions, opt => opt.MapFrom(src => GetFactions(src)))
            .ForMember(dest => dest.CulturalInstitutions, opt => opt.MapFrom(src => GetCulturalInstitutions(src)))
            // Reverse read-only lists
            .ForMember(dest => dest.MilitaryOrganizations, opt => opt.MapFrom(src => GetMilitaryOrganizations(src)))
            .ForMember(dest => dest.TradeRoutes, opt => opt.MapFrom(src => GetTradeRoutes(src)))
            .ForMember(dest => dest.Creatures, opt => opt.MapFrom(src => GetInhabitingCreatures(src)));
    }

    private static string? GetContinentSpecifics(Location location) => location is Continent c ? c.ContinentSpecifics : null;

    private static string? GetRegionSpecifics(Location location) => location is Region r ? r.RegionSpecifics : null;

    private static int? GetGovernmentSystemId(Location location) => location switch
    {
        Country country => country.GovernmentSystemId,
        City city => city.GovernmentSystemId,
        _ => null
    };

    private static int? GetLegalSystemId(Location location) => location switch
    {
        Country country => country.LegalSystemId,
        City city => city.LegalSystemId,
        _ => null
    };

    private static int? GetEducationSystemId(Location location) => location switch
    {
        Country country => country.EducationSystemId,
        City city => city.EducationSystemId,
        _ => null
    };

    private static int? GetEconomicSystemId(Location location) => location switch
    {
        Country country => country.EconomicSystemId,
        City city => city.EconomicSystemId,
        _ => null
    };

    private static bool? GetIsCapital(Location location) => location is City city ? city.IsCapital : null;

    private static string? GetDistrictType(Location location) => location is District d ? d.DistrictType : null;
    private static string? GetLandmarkType(Location location) => location is Landmark l ? l.LandmarkType : null;

    private static ReferenceDto? GetGovernmentSystem(Location location)
    {
        var governmentSystem = location switch
        {
            Country country => country.GovernmentSystem,
            City city => city.GovernmentSystem,
            _ => null
        };
        return governmentSystem == null ? null : new ReferenceDto { Id = governmentSystem.Id, Name = governmentSystem.Name };
    }

    private static ReferenceDto? GetLegalSystem(Location location)
    {
        var legalSystem = location switch
        {
            Country country => country.LegalSystem,
            City city => city.LegalSystem,
            _ => null
        };
        return legalSystem == null ? null : new ReferenceDto { Id = legalSystem.Id, Name = legalSystem.Name };
    }

    private static ReferenceDto? GetEducationSystem(Location location)
    {
        var educationSystem = location switch
        {
            Country country => country.EducationSystem,
            City city => city.EducationSystem,
            _ => null
        };
        return educationSystem == null ? null : new ReferenceDto { Id = educationSystem.Id, Name = educationSystem.Name };
    }

    private static ReferenceDto? GetEconomicSystem(Location location)
    {
        var economicSystem = location switch
        {
            Country country => country.EconomicSystem,
            City city => city.EconomicSystem,
            _ => null
        };
        return economicSystem == null ? null : new ReferenceDto { Id = economicSystem.Id, Name = economicSystem.Name };
    }

    private static List<ReferenceDto> GetNativeSpecies(Location location) => location is Region region
        ? region.OriginOfSapientSpecies
            .Where(rs => rs.SapientSpecies != null)
            .Select(rs => new ReferenceDto { Id = rs.SapientSpecies!.Id, Name = rs.SapientSpecies.Name })
            .ToList()
        : new List<ReferenceDto>();

    private static string? GetUniqueFeatures(Location location) => location is Ecosystem e ? e.UniqueFeatures : null;

    private static double? GetWaterDepth(Location location) => location is WaterEcosystem w ? w.WaterDepth : null;

    private static double? GetVolume(Location location) => location is LakeEcosystem l ? l.Volume : null;
    private static double? GetMaxDepth(Location location) => location is LakeEcosystem l ? l.MaxDepth : null;
    private static bool? GetIsFreshwater(Location location) => location is LakeEcosystem l ? l.IsFreshwater : null;

    private static double? GetRiverLength(Location location) => location is RiverEcosystem r ? r.RiverLength : null;
    private static int? GetSourceLocationId(Location location) => location is RiverEcosystem r ? r.SourceLocationId : null;
    private static int? GetMouthLocationId(Location location) => location is RiverEcosystem r ? r.MouthLocationId : null;

    private static double? GetMaxElevation(Location location) => location is MountainEcosystem m ? m.MaxElevation : null;
    private static double? GetProminence(Location location) => location is MountainEcosystem m ? m.Prominence : null;

    private static double? GetMountainRangeLength(Location location) => location is MountainRange r ? r.MountainRangeLength : null;

    private static bool? GetIsSaltwater(Location location) => location is SwampEcosystem s ? s.IsSaltwater : null;

    private static DesertType? GetDesertKind(Location location) => location is DesertEcosystem d ? d.DesertKind : null;
    private static ForestType? GetForestKind(Location location) => location is ForestEcosystem f ? f.ForestKind : null;
    private static double? GetCaveDepth(Location location) => location is CaveEcosystem c ? c.CaveDepth : null;
    private static CaveType? GetCaveKind(Location location) => location is CaveEcosystem c ? c.CaveKind : null;
    private static GrasslandType? GetGrasslandKind(Location location) => location is GrasslandEcosystem g ? g.GrasslandKind : null;

    private static ReferenceDto? GetSourceLocation(Location location) => location is RiverEcosystem { SourceLocation: { } source }
        ? new ReferenceDto { Id = source.Id, Name = source.Name }
        : null;

    private static ReferenceDto? GetMouthLocation(Location location) => location is RiverEcosystem { MouthLocation: { } mouth }
        ? new ReferenceDto { Id = mouth.Id, Name = mouth.Name }
        : null;

    // ---- Country/City cross-links (editable) ----

    private static List<ReferenceDto> GetIndustries(Location location) => location switch
    {
        Country c => c.Industries.Where(x => x.Industry != null).Select(x => new ReferenceDto { Id = x.Industry!.Id, Name = x.Industry.Name }).ToList(),
        City c => c.Industries.Where(x => x.Industry != null).Select(x => new ReferenceDto { Id = x.Industry!.Id, Name = x.Industry.Name }).ToList(),
        _ => new List<ReferenceDto>()
    };

    private static List<ReferenceDto> GetCorporations(Location location) => location switch
    {
        Country c => c.Corporations.Where(x => x.Corporation != null).Select(x => new ReferenceDto { Id = x.Corporation!.Id, Name = x.Corporation.Name }).ToList(),
        City c => c.Corporations.Where(x => x.Corporation != null).Select(x => new ReferenceDto { Id = x.Corporation!.Id, Name = x.Corporation.Name }).ToList(),
        _ => new List<ReferenceDto>()
    };

    private static List<ReferenceDto> GetGuilds(Location location) => location switch
    {
        Country c => c.Guilds.Where(x => x.Guild != null).Select(x => new ReferenceDto { Id = x.Guild!.Id, Name = x.Guild.Name }).ToList(),
        City c => c.Guilds.Where(x => x.Guild != null).Select(x => new ReferenceDto { Id = x.Guild!.Id, Name = x.Guild.Name }).ToList(),
        _ => new List<ReferenceDto>()
    };

    private static List<ReferenceDto> GetPoliticalParties(Location location) => location switch
    {
        Country c => c.PoliticalParties.Where(x => x.PoliticalParty != null).Select(x => new ReferenceDto { Id = x.PoliticalParty!.Id, Name = x.PoliticalParty.Name }).ToList(),
        City c => c.PoliticalParties.Where(x => x.PoliticalParty != null).Select(x => new ReferenceDto { Id = x.PoliticalParty!.Id, Name = x.PoliticalParty.Name }).ToList(),
        _ => new List<ReferenceDto>()
    };

    private static List<ReferenceDto> GetNations(Location location) => location switch
    {
        Country c => c.Nations.Where(x => x.Nation != null).Select(x => new ReferenceDto { Id = x.Nation!.Id, Name = x.Nation.Name }).ToList(),
        City c => c.Nations.Where(x => x.Nation != null).Select(x => new ReferenceDto { Id = x.Nation!.Id, Name = x.Nation.Name }).ToList(),
        _ => new List<ReferenceDto>()
    };

    private static List<ReferenceDto> GetCultures(Location location) => location switch
    {
        Country c => c.PredominantCultures.Where(x => x.Culture != null).Select(x => new ReferenceDto { Id = x.Culture!.Id, Name = x.Culture.Name }).ToList(),
        City c => c.PredominantCultures.Where(x => x.Culture != null).Select(x => new ReferenceDto { Id = x.Culture!.Id, Name = x.Culture.Name }).ToList(),
        _ => new List<ReferenceDto>()
    };

    private static List<ReferenceDto> GetReligions(Location location) => location switch
    {
        Country c => c.Religions.Where(x => x.Religion != null).Select(x => new ReferenceDto { Id = x.Religion!.Id, Name = x.Religion.Name }).ToList(),
        City c => c.Religions.Where(x => x.Religion != null).Select(x => new ReferenceDto { Id = x.Religion!.Id, Name = x.Religion.Name }).ToList(),
        _ => new List<ReferenceDto>()
    };

    private static List<ReferenceDto> GetFactions(Location location) => location is Country country
        ? country.Factions.Where(x => x.Faction != null).Select(x => new ReferenceDto { Id = x.Faction!.Id, Name = x.Faction.Name }).ToList()
        : new List<ReferenceDto>();

    private static List<ReferenceDto> GetCulturalInstitutions(Location location) => location is City city
        ? city.CulturalInstitutions.Where(x => x.CulturalInstitution != null).Select(x => new ReferenceDto { Id = x.CulturalInstitution!.Id, Name = x.CulturalInstitution.Name }).ToList()
        : new List<ReferenceDto>();

    // ---- Reverse read-only lists ----

    private static List<ReferenceDto> GetMilitaryOrganizations(Location location) => location is Country country
        ? country.MilitaryOrganizations.Where(x => x.MilitaryOrganization != null).Select(x => new ReferenceDto { Id = x.MilitaryOrganization!.Id, Name = x.MilitaryOrganization.Name }).ToList()
        : new List<ReferenceDto>();

    private static List<ReferenceDto> GetInhabitingCreatures(Location location) => location is City city
        ? city.InhabitingCreatures.Where(x => x.Creature != null).Select(x => new ReferenceDto { Id = x.Creature!.Id, Name = x.Creature.Name }).ToList()
        : new List<ReferenceDto>();

    private static List<ReferenceDto> GetTradeRoutes(Location location) => location is Country or City
        ? location.TradeRouteLinks.Where(x => x.TradeRoute != null).Select(x => new ReferenceDto { Id = x.TradeRoute!.Id, Name = x.TradeRoute.Name }).ToList()
        : new List<ReferenceDto>();
}
