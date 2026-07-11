using AutoMapper;
using ChronicleKeeper.Core.DTOs.ContentReferenceLink;
using ChronicleKeeper.Core.Entities.Content;

public class ContentReferenceLinkProfile : Profile
{
    public ContentReferenceLinkProfile()
    {
        CreateMap<Reference, ContentReferenceLinkDto>();
        CreateMap<ContentReferenceLinkCreateDto, Reference>();
        CreateMap<ContentReferenceLinkUpdateDto, Reference>();
    }
}
