using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.EducationSystem;
using ChronicleKeeper.Core.Entities.Social.Education;

public class EducationSystemProfile : Profile
{
    public EducationSystemProfile()
    {
        CreateMap<EducationSystem, EducationSystemDto>();
        CreateMap<EducationSystemCreateDto, EducationSystem>();
        CreateMap<EducationSystemUpdateDto, EducationSystem>();

        CreateMap<EducationSystem, EducationSystemDetailsDto>()
            .ForMember(dest => dest.Schools, opt => opt.MapFrom(src => src.Schools
                .Select(s => new ReferenceDto { Id = s.Id, Name = s.Name })))
            .ForMember(dest => dest.Universities, opt => opt.MapFrom(src => src.Universities
                .Select(u => new ReferenceDto { Id = u.Id, Name = u.Name })));
    }
}
