using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.CultureDetails;
using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeperAPI.Mapping.CultureDetails
{
    public class CultureDetailsProfilesA : Profile
    {
        public CultureDetailsProfilesA()
        {
            // ---- Custom ----
            CreateMap<Custom, CustomDto>();
            CreateMap<CustomCreateDto, Custom>();
            CreateMap<CustomUpdateDto, Custom>();
            CreateMap<Custom, CustomDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }));

            // ---- ArtForm ----
            CreateMap<ArtForm, ArtFormDto>();
            CreateMap<ArtFormCreateDto, ArtForm>();
            CreateMap<ArtFormUpdateDto, ArtForm>();
            CreateMap<ArtForm, ArtFormDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }));

            // ---- Cuisine ----
            CreateMap<Cuisine, CuisineDto>();
            CreateMap<CuisineCreateDto, Cuisine>();
            CreateMap<CuisineUpdateDto, Cuisine>();
            CreateMap<Cuisine, CuisineDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }));

            // ---- Clothing ----
            CreateMap<Clothing, ClothingDto>();
            CreateMap<ClothingCreateDto, Clothing>();
            CreateMap<ClothingUpdateDto, Clothing>();
            CreateMap<Clothing, ClothingDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }));

            // ---- Tradition ----
            CreateMap<Tradition, TraditionDto>();
            CreateMap<TraditionCreateDto, Tradition>();
            CreateMap<TraditionUpdateDto, Tradition>();
            CreateMap<Tradition, TraditionDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture == null ? null : new ReferenceDto { Id = s.Culture.Id, Name = s.Culture.Name }))
                .ForMember(d => d.Religion, o => o.MapFrom(s => s.Religion == null ? null : new ReferenceDto { Id = s.Religion.Id, Name = s.Religion.Name }));
        }
    }
}
