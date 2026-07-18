using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.CultureDetails;
using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeperAPI.Mapping.CultureDetails
{
    public class CultureDetailsProfilesB : Profile
    {
        public CultureDetailsProfilesB()
        {
            // ---- ArchitectureStyle ----
            CreateMap<ArchitectureStyle, ArchitectureStyleDto>();
            CreateMap<ArchitectureStyleCreateDto, ArchitectureStyle>();
            CreateMap<ArchitectureStyleUpdateDto, ArchitectureStyle>();
            CreateMap<ArchitectureStyle, ArchitectureStyleDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }))
                .ForMember(d => d.TypicalLocations, o => o.MapFrom(s => s.TypicalLocations.Where(x => x.Location != null).Select(x => new ReferenceDto { Id = x.Location!.Id, Name = x.Location.Name })));

            // ---- Folklore ----
            CreateMap<Folklore, FolkloreDto>();
            CreateMap<FolkloreCreateDto, Folklore>();
            CreateMap<FolkloreUpdateDto, Folklore>();
            CreateMap<Folklore, FolkloreDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }))
                .ForMember(d => d.RelatedEvents, o => o.MapFrom(s => s.RelatedEvents.Where(x => x.TimelineEvent != null).Select(x => new ReferenceDto { Id = x.TimelineEvent!.Id, Name = x.TimelineEvent.Name })))
                .ForMember(d => d.OriginatedFromSpecies, o => o.MapFrom(s => s.OriginatedFromSpecies.Where(x => x.SapientSpecies != null).Select(x => new ReferenceDto { Id = x.SapientSpecies!.Id, Name = x.SapientSpecies.Name })));

            // ---- Myth ----
            CreateMap<Myth, MythDto>();
            CreateMap<MythCreateDto, Myth>();
            CreateMap<MythUpdateDto, Myth>();
            CreateMap<Myth, MythDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }))
                .ForMember(d => d.Religion, o => o.MapFrom(s => s.Religion == null ? null : new ReferenceDto { Id = s.Religion.Id, Name = s.Religion.Name }));

            // ---- CulturalFestival ----
            CreateMap<CulturalFestival, CulturalFestivalDto>();
            CreateMap<CulturalFestivalCreateDto, CulturalFestival>();
            CreateMap<CulturalFestivalUpdateDto, CulturalFestival>();
            CreateMap<CulturalFestival, CulturalFestivalDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location == null ? null : new ReferenceDto { Id = s.Location.Id, Name = s.Location.Name }));

            // ---- CulturalInstitution ----
            CreateMap<CulturalInstitution, CulturalInstitutionDto>();
            CreateMap<CulturalInstitutionCreateDto, CulturalInstitution>();
            CreateMap<CulturalInstitutionUpdateDto, CulturalInstitution>();
            CreateMap<CulturalInstitution, CulturalInstitutionDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City == null ? null : new ReferenceDto { Id = s.City.Id, Name = s.City.Name }));
        }
    }
}
