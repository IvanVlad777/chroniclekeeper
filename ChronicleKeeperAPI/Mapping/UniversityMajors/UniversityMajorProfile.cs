using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.UniversityMajor;
using ChronicleKeeper.Core.Entities.Social.Education;

public class UniversityMajorProfile : Profile
{
    public UniversityMajorProfile()
    {
        CreateMap<UniversityMajor, UniversityMajorDto>();
        CreateMap<UniversityMajorCreateDto, UniversityMajor>();
        CreateMap<UniversityMajorUpdateDto, UniversityMajor>();

        CreateMap<UniversityMajor, UniversityMajorDetailsDto>()
            .ForMember(d => d.University, o => o.MapFrom(s => s.University == null
                ? null
                : new ReferenceDto { Id = s.University.Id, Name = s.University.Name }))
            .ForMember(d => d.Professors, o => o.MapFrom(s => s.Professors
                .Where(p => p.Character != null)
                .Select(p => new ReferenceDto { Id = p.Character!.Id, Name = p.Character.Name })))
            .ForMember(d => d.Students, o => o.MapFrom(s => s.Students
                .Where(st => st.Character != null)
                .Select(st => new ReferenceDto { Id = st.Character!.Id, Name = st.Character.Name })));
    }
}
