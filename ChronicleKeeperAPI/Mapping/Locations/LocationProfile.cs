using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Location;
using ChronicleKeeper.Core.Entities.Geography;

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
            .ForMember(d => d.IsCapital, opt => opt.MapFrom(src => GetIsCapital(src)))
            .ForMember(d => d.DistrictType, opt => opt.MapFrom(src => GetDistrictType(src)));

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
            .ForMember(dest => dest.Schools, opt => opt.MapFrom(src => src.Schools
                .Select(s => new ReferenceDto { Id = s.Id, Name = s.Name })))
            .ForMember(dest => dest.NativeSpecies, opt => opt.MapFrom(src => GetNativeSpecies(src)));
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

    private static bool? GetIsCapital(Location location) => location is City city ? city.IsCapital : null;

    private static string? GetDistrictType(Location location) => location is District d ? d.DistrictType : null;

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

    private static List<ReferenceDto> GetNativeSpecies(Location location) => location is Region region
        ? region.OriginOfSapientSpecies
            .Where(rs => rs.SapientSpecies != null)
            .Select(rs => new ReferenceDto { Id = rs.SapientSpecies!.Id, Name = rs.SapientSpecies.Name })
            .ToList()
        : new List<ReferenceDto>();
}
