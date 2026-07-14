using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.ExtractionMethod;
using ChronicleKeeper.Core.DTOs.Industry;
using ChronicleKeeper.Core.DTOs.NaturalResource;
using ChronicleKeeper.Core.DTOs.TradeRoute;
using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeperAPI.Mapping.Economy
{
    public class IndustryProfile : Profile
    {
        public IndustryProfile()
        {
            CreateMap<Industry, IndustryDto>();
            CreateMap<IndustryCreateDto, Industry>();
            CreateMap<IndustryUpdateDto, Industry>();

            CreateMap<Industry, IndustryDetailsDto>()
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
        }
    }

    public class ExtractionMethodProfile : Profile
    {
        public ExtractionMethodProfile()
        {
            CreateMap<ExtractionMethod, ExtractionMethodDto>();
            CreateMap<ExtractionMethodCreateDto, ExtractionMethod>();
            CreateMap<ExtractionMethodUpdateDto, ExtractionMethod>();

            CreateMap<ExtractionMethod, ExtractionMethodDetailsDto>()
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
                .ForMember(dest => dest.ResourcesExtracted, opt => opt.MapFrom(src => src.ResourcesExtracted
                    .Select(r => new ReferenceDto { Id = r.Id, Name = r.Name })));
        }
    }

    public class NaturalResourceProfile : Profile
    {
        public NaturalResourceProfile()
        {
            CreateMap<NaturalResource, NaturalResourceDto>();
            CreateMap<NaturalResourceCreateDto, NaturalResource>();
            CreateMap<NaturalResourceUpdateDto, NaturalResource>();

            CreateMap<NaturalResource, NaturalResourceDetailsDto>()
                .ForMember(dest => dest.ExtractionMethod, opt => opt.MapFrom(src => src.ExtractionMethod == null ? null : new ReferenceDto { Id = src.ExtractionMethod.Id, Name = src.ExtractionMethod.Name }))
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations
                    .Select(nl => new ReferenceDto { Id = nl.Location!.Id, Name = nl.Location!.Name })))
                .ForMember(dest => dest.ExportRoutes, opt => opt.MapFrom(src => src.ExportRoutes
                    .Select(tr => new ReferenceDto { Id = tr.TradeRoute!.Id, Name = tr.TradeRoute!.Name })));
        }
    }

    public class TradeRouteProfile : Profile
    {
        public TradeRouteProfile()
        {
            CreateMap<TradeRoute, TradeRouteDto>();
            CreateMap<TradeRouteCreateDto, TradeRoute>();
            CreateMap<TradeRouteUpdateDto, TradeRoute>();

            CreateMap<TradeRoute, TradeRouteDetailsDto>()
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations
                    .Select(tl => new ReferenceDto { Id = tl.Location!.Id, Name = tl.Location!.Name })))
                .ForMember(dest => dest.ResourcesTraded, opt => opt.MapFrom(src => src.ResourcesTraded
                    .Select(tr => new ReferenceDto { Id = tr.NaturalResource!.Id, Name = tr.NaturalResource!.Name })));
        }
    }
}
