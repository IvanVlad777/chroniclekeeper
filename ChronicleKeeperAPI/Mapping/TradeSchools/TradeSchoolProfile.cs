using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.TradeSchool;
using ChronicleKeeper.Core.Entities.Professions;

public class TradeSchoolProfile : Profile
{
    public TradeSchoolProfile()
    {
        CreateMap<TradeSchool, TradeSchoolDto>();
        CreateMap<TradeSchoolCreateDto, TradeSchool>();
        CreateMap<TradeSchoolUpdateDto, TradeSchool>();

        CreateMap<TradeSchool, TradeSchoolDetailsDto>()
            .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects))
            .ForMember(dest => dest.Alumni, opt => opt.MapFrom(src => src.Alumni
                .Select(e => new ReferenceDto { Id = e.Id, Name = e.Name })))
            .ForMember(dest => dest.TrainedProfessions, opt => opt.MapFrom(src => src.TrainedProfessions
                .Select(pt => new ReferenceDto { Id = pt.Profession!.Id, Name = pt.Profession.Name })))
            .ForMember(dest => dest.Apprenticeships, opt => opt.MapFrom(src => src.Apprenticeships
                .Select(a => new ReferenceDto { Id = a.Id, Name = a.Name })));
    }
}
