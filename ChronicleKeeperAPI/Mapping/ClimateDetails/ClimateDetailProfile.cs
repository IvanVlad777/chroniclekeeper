using AutoMapper;
using ChronicleKeeper.Core.DTOs.ClimateDetail;
using ChronicleKeeper.Core.Entities.Geography.Climate;

public class ClimateDetailProfile : Profile
{
    public ClimateDetailProfile()
    {
        CreateMap<ClimateDetail, ClimateDetailDto>();
        CreateMap<ClimateDetailCreateDto, ClimateDetail>();
        CreateMap<ClimateDetailUpdateDto, ClimateDetail>();
    }
}
