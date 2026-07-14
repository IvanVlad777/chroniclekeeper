using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.BankingSystem;
using ChronicleKeeper.Core.DTOs.Currency;
using ChronicleKeeper.Core.DTOs.EconomicSystem;
using ChronicleKeeper.Core.DTOs.TaxationSystem;
using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeperAPI.Mapping.Economy
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, CurrencyDto>();
            CreateMap<CurrencyCreateDto, Currency>();
            CreateMap<CurrencyUpdateDto, Currency>();

            CreateMap<Currency, CurrencyDetailsDto>()
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
        }
    }

    public class BankingSystemProfile : Profile
    {
        public BankingSystemProfile()
        {
            CreateMap<BankingSystem, BankingSystemDto>();
            CreateMap<BankingSystemCreateDto, BankingSystem>();
            CreateMap<BankingSystemUpdateDto, BankingSystem>();

            CreateMap<BankingSystem, BankingSystemDetailsDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency == null ? null : new ReferenceDto { Id = src.Currency.Id, Name = src.Currency.Name }))
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
        }
    }

    public class TaxationSystemProfile : Profile
    {
        public TaxationSystemProfile()
        {
            CreateMap<TaxationSystem, TaxationSystemDto>();
            CreateMap<TaxationSystemCreateDto, TaxationSystem>();
            CreateMap<TaxationSystemUpdateDto, TaxationSystem>();

            CreateMap<TaxationSystem, TaxationSystemDetailsDto>()
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
        }
    }

    public class EconomicSystemProfile : Profile
    {
        public EconomicSystemProfile()
        {
            CreateMap<EconomicSystem, EconomicSystemDto>();
            CreateMap<EconomicSystemCreateDto, EconomicSystem>();
            CreateMap<EconomicSystemUpdateDto, EconomicSystem>();

            CreateMap<EconomicSystem, EconomicSystemDetailsDto>()
                .ForMember(dest => dest.TaxationSystem, opt => opt.MapFrom(src => src.TaxationSystem == null ? null : new ReferenceDto { Id = src.TaxationSystem.Id, Name = src.TaxationSystem.Name }))
                .ForMember(dest => dest.BankingSystem, opt => opt.MapFrom(src => src.BankingSystem == null ? null : new ReferenceDto { Id = src.BankingSystem.Id, Name = src.BankingSystem.Name }))
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
        }
    }
}
