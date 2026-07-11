using AutoMapper;
using ChronicleKeeper.Core.DTOs.ReligiousEducation;
using ChronicleKeeper.Core.Entities.Social.Education;

public class ReligiousEducationProfile : Profile
{
    public ReligiousEducationProfile()
    {
        CreateMap<ReligiousEducation, ReligiousEducationDto>();
        CreateMap<ReligiousEducationCreateDto, ReligiousEducation>();
        CreateMap<ReligiousEducationUpdateDto, ReligiousEducation>();
    }
}
