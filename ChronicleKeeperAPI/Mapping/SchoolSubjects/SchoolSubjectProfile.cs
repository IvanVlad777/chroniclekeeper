using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.SchoolSubject;
using ChronicleKeeper.Core.Entities.Social.Education;

public class SchoolSubjectProfile : Profile
{
    public SchoolSubjectProfile()
    {
        CreateMap<SchoolSubject, SchoolSubjectDto>();
        CreateMap<SchoolSubjectCreateDto, SchoolSubject>();
        CreateMap<SchoolSubjectUpdateDto, SchoolSubject>();

        CreateMap<SchoolSubject, SchoolSubjectDetailsDto>()
            .ForMember(d => d.School, o => o.MapFrom(s => s.School == null
                ? null
                : new ReferenceDto { Id = s.School.Id, Name = s.School.Name }))
            .ForMember(d => d.Teachers, o => o.MapFrom(s => s.Teachers
                .Where(t => t.Character != null)
                .Select(t => new ReferenceDto { Id = t.Character!.Id, Name = t.Character.Name })));
    }
}
