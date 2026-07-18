using AutoMapper;
using ChronicleKeeper.Core.DTOs.PrivilegeLaw;
using ChronicleKeeper.Core.Entities.Social.Structure;

public class PrivilegeLawProfile : Profile
{
    public PrivilegeLawProfile()
    {
        CreateMap<PrivilegeLaw, PrivilegeLawDto>();
        CreateMap<PrivilegeLawCreateDto, PrivilegeLaw>();
        CreateMap<PrivilegeLawUpdateDto, PrivilegeLaw>();
    }
}
