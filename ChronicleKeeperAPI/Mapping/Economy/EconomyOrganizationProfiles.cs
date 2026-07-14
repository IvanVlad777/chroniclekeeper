using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.CorporateLeadership;
using ChronicleKeeper.Core.DTOs.Corporation;
using ChronicleKeeper.Core.DTOs.Guild;
using ChronicleKeeper.Core.DTOs.GuildRank;
using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeperAPI.Mapping.Economy
{
    public class GuildProfile : Profile
    {
        public GuildProfile()
        {
            CreateMap<Guild, GuildDto>();
            CreateMap<GuildCreateDto, Guild>();
            CreateMap<GuildUpdateDto, Guild>();

            CreateMap<Guild, GuildDetailsDto>()
                .ForMember(dest => dest.TaxationSystem, opt => opt.MapFrom(src => src.TaxationSystem == null ? null : new ReferenceDto { Id = src.TaxationSystem.Id, Name = src.TaxationSystem.Name }))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Industry == null ? null : new ReferenceDto { Id = src.Industry.Id, Name = src.Industry.Name }))
                .ForMember(dest => dest.LegalSystem, opt => opt.MapFrom(src => src.LegalSystem == null ? null : new ReferenceDto { Id = src.LegalSystem.Id, Name = src.LegalSystem.Name }))
                .ForMember(dest => dest.EducationSystem, opt => opt.MapFrom(src => src.EducationSystem == null ? null : new ReferenceDto { Id = src.EducationSystem.Id, Name = src.EducationSystem.Name }))
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
                .ForMember(dest => dest.GuildRanks, opt => opt.MapFrom(src => src.GuildRanks))
                .ForMember(dest => dest.Factions, opt => opt.MapFrom(src => src.Factions
                    .Select(gf => new ReferenceDto { Id = gf.Faction!.Id, Name = gf.Faction!.Name })))
                .ForMember(dest => dest.MemberProfessions, opt => opt.MapFrom(src => src.MemberProfessions
                    .Select(gp => new ReferenceDto { Id = gp.Profession!.Id, Name = gp.Profession!.Name })))
                .ForMember(dest => dest.SocialClasses, opt => opt.MapFrom(src => src.SocialClasses
                    .Select(gs => new ReferenceDto { Id = gs.SocialClass!.Id, Name = gs.SocialClass!.Name })));
        }
    }

    public class GuildRankProfile : Profile
    {
        public GuildRankProfile()
        {
            CreateMap<GuildRank, GuildRankDto>();
            CreateMap<GuildRankCreateDto, GuildRank>();
            CreateMap<GuildRankUpdateDto, GuildRank>();
        }
    }

    public class CorporationProfile : Profile
    {
        public CorporationProfile()
        {
            CreateMap<Corporation, CorporationDto>();
            CreateMap<CorporationCreateDto, Corporation>();
            CreateMap<CorporationUpdateDto, Corporation>();

            CreateMap<Corporation, CorporationDetailsDto>()
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Industry == null ? null : new ReferenceDto { Id = src.Industry.Id, Name = src.Industry.Name }))
                .ForMember(dest => dest.TaxationSystem, opt => opt.MapFrom(src => src.TaxationSystem == null ? null : new ReferenceDto { Id = src.TaxationSystem.Id, Name = src.TaxationSystem.Name }))
                .ForMember(dest => dest.BankingSystem, opt => opt.MapFrom(src => src.BankingSystem == null ? null : new ReferenceDto { Id = src.BankingSystem.Id, Name = src.BankingSystem.Name }))
                .ForMember(dest => dest.ParentCorporation, opt => opt.MapFrom(src => src.ParentCorporation == null ? null : new ReferenceDto { Id = src.ParentCorporation.Id, Name = src.ParentCorporation.Name }))
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
                .ForMember(dest => dest.Subsidiaries, opt => opt.MapFrom(src => src.Subsidiaries
                    .Select(s => new ReferenceDto { Id = s.Id, Name = s.Name })))
                .ForMember(dest => dest.Leadership, opt => opt.MapFrom(src => src.Leadership))
                .ForMember(dest => dest.Factions, opt => opt.MapFrom(src => src.Factions
                    .Select(cf => new ReferenceDto { Id = cf.Faction!.Id, Name = cf.Faction!.Name })))
                .ForMember(dest => dest.MemberProfessions, opt => opt.MapFrom(src => src.MemberProfessions
                    .Select(cp => new ReferenceDto { Id = cp.Profession!.Id, Name = cp.Profession!.Name })));
        }
    }

    public class CorporateLeadershipProfile : Profile
    {
        public CorporateLeadershipProfile()
        {
            CreateMap<CorporateLeadership, CorporateLeadershipDto>()
                .ForMember(dest => dest.ProfessionName, opt => opt.MapFrom(src => src.Profession == null ? null : src.Profession.Name))
                .ForMember(dest => dest.CharacterName, opt => opt.MapFrom(src => src.Character == null ? null : src.Character.Name));
            CreateMap<CorporateLeadershipCreateDto, CorporateLeadership>();
            CreateMap<CorporateLeadershipUpdateDto, CorporateLeadership>();
        }
    }
}
