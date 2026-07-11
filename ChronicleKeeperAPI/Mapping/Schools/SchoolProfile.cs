using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.School;
using ChronicleKeeper.Core.Entities.Social.Education;

public class SchoolProfile : Profile
{
    public SchoolProfile()
    {
        CreateMap<School, SchoolDto>()
            .ForMember(dest => dest.SchoolType, opt => opt.MapFrom(src =>
                src is ChronicleKeeper.Core.Entities.Professions.TradeSchool ? "TradeSchool" : "School"));

        CreateMap<SchoolCreateDto, School>();
        CreateMap<SchoolUpdateDto, School>();

        CreateMap<School, SchoolDetailsDto>()
            .ForMember(dest => dest.SchoolType, opt => opt.MapFrom(src =>
                src is ChronicleKeeper.Core.Entities.Professions.TradeSchool ? "TradeSchool" : "School"))
            .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects))
            .ForMember(dest => dest.Alumni, opt => opt.MapFrom(src => src.Alumni
                .Select(e => new ReferenceDto { Id = e.Id, Name = e.Name })));
    }
}
