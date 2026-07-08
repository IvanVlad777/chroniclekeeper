using AutoMapper;
using ChronicleKeeper.Core.DTOs.Tag;
using TagEntity = ChronicleKeeper.Core.Entities.Tags.Tag;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<TagEntity, TagDto>();
        CreateMap<TagCreateDto, TagEntity>();
        CreateMap<TagUpdateDto, TagEntity>();
    }
}
