using AutoMapper;
using ChronicleKeeper.Core.DTOs.Specialisation;
using ChronicleKeeper.Core.Entities.Professions;

public class SpecialisationProfile : Profile
{
    public SpecialisationProfile()
    {
        CreateMap<Specialisation, SpecialisationDto>();
        CreateMap<SpecialisationCreateDto, Specialisation>();
        CreateMap<SpecialisationUpdateDto, Specialisation>();
    }
}
