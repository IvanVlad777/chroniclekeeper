using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.University;
using ChronicleKeeper.Core.Entities.Social.Education;

public class UniversityProfile : Profile
{
    public UniversityProfile()
    {
        CreateMap<University, UniversityDto>();
        CreateMap<UniversityCreateDto, University>();
        CreateMap<UniversityUpdateDto, University>();

        CreateMap<University, UniversityDetailsDto>()
            .ForMember(dest => dest.Majors, opt => opt.MapFrom(src => src.Majors))
            .ForMember(dest => dest.Alumni, opt => opt.MapFrom(src => src.Alumni
                .Select(e => new ReferenceDto { Id = e.Id, Name = e.Name })));
    }
}
