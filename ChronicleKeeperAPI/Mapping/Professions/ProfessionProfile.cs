using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Profession;
using ChronicleKeeper.Core.Entities.Professions;

public class ProfessionProfile : Profile
{
    public ProfessionProfile()
    {
        CreateMap<Profession, ProfessionDto>();
        CreateMap<ProfessionCreateDto, Profession>();
        CreateMap<ProfessionUpdateDto, Profession>();

        CreateMap<Profession, ProfessionDetailsDto>()
            .ForMember(dest => dest.JobRanks, opt => opt.MapFrom(src => src.JobRanks))
            .ForMember(dest => dest.Apprenticeships, opt => opt.MapFrom(src => src.Apprenticeships))
            .ForMember(dest => dest.Specialisations, opt => opt.MapFrom(src => src.Specialisations))
            .ForMember(dest => dest.PracticedBySpecies, opt => opt.MapFrom(src => src.PracticedBySpecies
                .Select(ps => new ReferenceDto { Id = ps.SapientSpecies!.Id, Name = ps.SapientSpecies.Name })))
            .ForMember(dest => dest.SocialClasses, opt => opt.MapFrom(src => src.SocialClasses
                .Select(ps => new ReferenceDto { Id = ps.SocialClass!.Id, Name = ps.SocialClass.Name })))
            .ForMember(dest => dest.TradeSchools, opt => opt.MapFrom(src => src.TradeSchools
                .Select(pt => new ReferenceDto { Id = pt.TradeSchool!.Id, Name = pt.TradeSchool.Name })))
            .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => src.Characters
                .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })));
    }
}
