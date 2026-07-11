using AutoMapper;
using ChronicleKeeper.Core.DTOs.UniversityMajor;
using ChronicleKeeper.Core.Entities.Social.Education;

public class UniversityMajorProfile : Profile
{
    public UniversityMajorProfile()
    {
        CreateMap<UniversityMajor, UniversityMajorDto>();
        CreateMap<UniversityMajorCreateDto, UniversityMajor>();
        CreateMap<UniversityMajorUpdateDto, UniversityMajor>();
    }
}
