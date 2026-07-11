using AutoMapper;
using ChronicleKeeper.Core.DTOs.SchoolSubject;
using ChronicleKeeper.Core.Entities.Social.Education;

public class SchoolSubjectProfile : Profile
{
    public SchoolSubjectProfile()
    {
        CreateMap<SchoolSubject, SchoolSubjectDto>();
        CreateMap<SchoolSubjectCreateDto, SchoolSubject>();
        CreateMap<SchoolSubjectUpdateDto, SchoolSubject>();
    }
}
