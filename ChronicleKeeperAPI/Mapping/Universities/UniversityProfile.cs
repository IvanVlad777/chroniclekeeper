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
                .Select(e => new ReferenceDto { Id = e.Id, Name = e.Name })))
            .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students
                .Where(us => us.Character != null)
                .Select(us => new ReferenceDto { Id = us.Character!.Id, Name = us.Character.Name })))
            .ForMember(dest => dest.Professors, opt => opt.MapFrom(src => src.Professors
                .Where(up => up.Character != null)
                .Select(up => new ReferenceDto { Id = up.Character!.Id, Name = up.Character.Name })));
    }
}
