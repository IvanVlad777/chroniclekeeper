using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Location;
using ChronicleKeeper.Core.Entities.Geography;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, LocationDto>();
        CreateMap<LocationCreateDto, Location>();
        CreateMap<LocationUpdateDto, Location>();

        CreateMap<Location, LocationDetailsDto>()
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
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
    }
}
