using AutoMapper;
using ChronicleKeeper.Core.DTOs.LegalSystem;
using ChronicleKeeper.Core.Entities.Social.Politics;

public class LegalSystemProfile : Profile
{
    public LegalSystemProfile()
    {
        CreateMap<LegalSystem, LegalSystemDto>();
        CreateMap<LegalSystemCreateDto, LegalSystem>();
        CreateMap<LegalSystemUpdateDto, LegalSystem>();
        CreateMap<LegalSystem, LegalSystemDetailsDto>();
    }
}
