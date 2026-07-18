using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Library;
using ChronicleKeeper.Core.Entities.Social.Education;

public class LibraryProfile : Profile
{
    public LibraryProfile()
    {
        CreateMap<Library, LibraryDto>()
            .ForMember(dest => dest.University, opt => opt.MapFrom(src => src.University == null
                ? null
                : new ReferenceDto { Id = src.University.Id, Name = src.University.Name }))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location == null
                ? null
                : new ReferenceDto { Id = src.Location.Id, Name = src.Location.Name }));

        CreateMap<LibraryCreateDto, Library>();
        CreateMap<LibraryUpdateDto, Library>();

        CreateMap<Library, LibraryDetailsDto>()
            .ForMember(dest => dest.University, opt => opt.MapFrom(src => src.University == null
                ? null
                : new ReferenceDto { Id = src.University.Id, Name = src.University.Name }))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location == null
                ? null
                : new ReferenceDto { Id = src.Location.Id, Name = src.Location.Name }))
            .ForMember(dest => dest.Scholars, opt => opt.MapFrom(src => src.Scholars
                .Where(ls => ls.Character != null)
                .Select(ls => new ReferenceDto { Id = ls.Character!.Id, Name = ls.Character.Name })));
    }
}
