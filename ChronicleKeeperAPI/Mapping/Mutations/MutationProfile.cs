using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Mutation;
using ChronicleKeeper.Core.Entities.Miscellaneous;

public class MutationProfile : Profile
{
    public MutationProfile()
    {
        CreateMap<Mutation, MutationDto>();
        CreateMap<MutationCreateDto, Mutation>();
        CreateMap<MutationUpdateDto, Mutation>();

        CreateMap<Mutation, MutationDetailsDto>()
            .ForMember(dest => dest.MutantCreature, opt => opt.MapFrom(src => src.MutantCreature == null
                ? null
                : new ReferenceDto { Id = src.MutantCreature.Id, Name = src.MutantCreature.Name }))
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null
                ? null
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
    }
}
