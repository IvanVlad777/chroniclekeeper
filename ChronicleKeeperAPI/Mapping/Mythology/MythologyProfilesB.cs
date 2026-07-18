using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Mythology;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeperAPI.Mapping.Mythology
{
    public class MythologyProfilesB : Profile
    {
        public MythologyProfilesB()
        {
            // ---- ReligiousOrder ----
            CreateMap<ReligiousOrder, ReligiousOrderDto>();
            CreateMap<ReligiousOrderCreateDto, ReligiousOrder>();
            CreateMap<ReligiousOrderUpdateDto, ReligiousOrder>();
            CreateMap<ReligiousOrder, ReligiousOrderDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Religion, o => o.MapFrom(s => s.Religion == null ? null : new ReferenceDto { Id = s.Religion.Id, Name = s.Religion.Name }))
                .ForMember(d => d.ClergyTraining, o => o.MapFrom(s => s.ClergyTraining.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })))
                .ForMember(d => d.Deities, o => o.MapFrom(s => s.Deities.Where(x => x.Deity != null).Select(x => new ReferenceDto { Id = x.Deity!.Id, Name = x.Deity.Name })))
                .ForMember(d => d.Factions, o => o.MapFrom(s => s.Factions.Where(x => x.Faction != null).Select(x => new ReferenceDto { Id = x.Faction!.Id, Name = x.Faction.Name })));

            // ---- Deity ----
            CreateMap<Deity, DeityDto>();
            CreateMap<DeityCreateDto, Deity>();
            CreateMap<DeityUpdateDto, Deity>();
            CreateMap<Deity, DeityDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Religion, o => o.MapFrom(s => s.Religion == null ? null : new ReferenceDto { Id = s.Religion.Id, Name = s.Religion.Name }))
                .ForMember(d => d.SacredTexts, o => o.MapFrom(s => s.SacredTexts.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })))
                .ForMember(d => d.SacredSitesOfDeity, o => o.MapFrom(s => s.SacredSitesOfDeity.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })))
                .ForMember(d => d.MajorMyths, o => o.MapFrom(s => s.MajorMyths.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })))
                .ForMember(d => d.OrdersDedicatedToDeity, o => o.MapFrom(s => s.OrdersDedicatedToDeity.Where(x => x.ReligiousOrder != null).Select(x => new ReferenceDto { Id = x.ReligiousOrder!.Id, Name = x.ReligiousOrder.Name })))
                .ForMember(d => d.AlliedDeities, o => o.MapFrom(s => s.AlliedDeities.Where(x => x.AlliedDeity != null).Select(x => new ReferenceDto { Id = x.AlliedDeity!.Id, Name = x.AlliedDeity.Name })))
                .ForMember(d => d.RivalDeities, o => o.MapFrom(s => s.RivalDeities.Where(x => x.RivalDeity != null).Select(x => new ReferenceDto { Id = x.RivalDeity!.Id, Name = x.RivalDeity.Name })));
        }
    }
}
