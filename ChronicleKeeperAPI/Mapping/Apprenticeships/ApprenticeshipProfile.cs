using AutoMapper;
using ChronicleKeeper.Core.DTOs.Apprenticeship;
using ChronicleKeeper.Core.Entities.Professions;

public class ApprenticeshipProfile : Profile
{
    public ApprenticeshipProfile()
    {
        CreateMap<Apprenticeship, ApprenticeshipDto>()
            .ForMember(dest => dest.TradeSchoolName, opt => opt.MapFrom(src => src.TradeSchool != null ? src.TradeSchool.Name : null));
        CreateMap<ApprenticeshipCreateDto, Apprenticeship>();
        CreateMap<ApprenticeshipUpdateDto, Apprenticeship>();
    }
}
