using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.DiplomaticAgreement;
using ChronicleKeeper.Core.Entities.Social.Politics;

public class DiplomaticAgreementProfile : Profile
{
    public DiplomaticAgreementProfile()
    {
        CreateMap<DiplomaticAgreement, DiplomaticAgreementDto>();
        CreateMap<DiplomaticAgreementCreateDto, DiplomaticAgreement>();
        CreateMap<DiplomaticAgreementUpdateDto, DiplomaticAgreement>();

        CreateMap<DiplomaticAgreement, DiplomaticAgreementDetailsDto>()
            .ForMember(dest => dest.FirstNation, opt => opt.MapFrom(src => new ReferenceDto { Id = src.FirstNation.Id, Name = src.FirstNation.Name }))
            .ForMember(dest => dest.SecondNation, opt => opt.MapFrom(src => new ReferenceDto { Id = src.SecondNation.Id, Name = src.SecondNation.Name }));
    }
}
