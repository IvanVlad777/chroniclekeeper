using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Mythology;
using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeperAPI.Mapping.Mythology
{
    public class MythologyProfilesA : Profile
    {
        public MythologyProfilesA()
        {
            // ---- HolySite ----
            CreateMap<HolySite, HolySiteDto>();
            CreateMap<HolySiteCreateDto, HolySite>();
            CreateMap<HolySiteUpdateDto, HolySite>();
            CreateMap<HolySite, HolySiteDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Religion, o => o.MapFrom(s => s.Religion == null ? null : new ReferenceDto { Id = s.Religion.Id, Name = s.Religion.Name }))
                .ForMember(d => d.Deity, o => o.MapFrom(s => s.Deity == null ? null : new ReferenceDto { Id = s.Deity.Id, Name = s.Deity.Name }))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location == null ? null : new ReferenceDto { Id = s.Location.Id, Name = s.Location.Name }));

            // ---- ReligiousText ----
            CreateMap<ReligiousText, ReligiousTextDto>();
            CreateMap<ReligiousTextCreateDto, ReligiousText>();
            CreateMap<ReligiousTextUpdateDto, ReligiousText>();
            CreateMap<ReligiousText, ReligiousTextDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Religion, o => o.MapFrom(s => s.Religion == null ? null : new ReferenceDto { Id = s.Religion.Id, Name = s.Religion.Name }))
                .ForMember(d => d.Deity, o => o.MapFrom(s => s.Deity == null ? null : new ReferenceDto { Id = s.Deity.Id, Name = s.Deity.Name }));

            // ---- ReligiousFestival ----
            CreateMap<ReligiousFestival, ReligiousFestivalDto>();
            CreateMap<ReligiousFestivalCreateDto, ReligiousFestival>();
            CreateMap<ReligiousFestivalUpdateDto, ReligiousFestival>();
            CreateMap<ReligiousFestival, ReligiousFestivalDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Religion, o => o.MapFrom(s => s.Religion == null ? null : new ReferenceDto { Id = s.Religion.Id, Name = s.Religion.Name }))
                .ForMember(d => d.HolySite, o => o.MapFrom(s => s.HolySite == null ? null : new ReferenceDto { Id = s.HolySite.Id, Name = s.HolySite.Name }));
        }
    }
}
