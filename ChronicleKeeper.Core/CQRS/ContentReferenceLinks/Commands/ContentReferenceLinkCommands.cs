using ChronicleKeeper.Core.DTOs.ContentReferenceLink;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ContentReferenceLinks.Commands
{
    public class CreateContentReferenceLinkCommand : IRequest<ContentReferenceLinkDto>
    {
        public ContentReferenceLinkCreateDto ContentReferenceLinkCreateDto { get; set; } = new();
    }

    public class UpdateContentReferenceLinkCommand : IRequest<ContentReferenceLinkDto>
    {
        public int Id { get; set; }
        public ContentReferenceLinkUpdateDto ContentReferenceLinkUpdateDto { get; set; } = new();
    }

    public class DeleteContentReferenceLinkCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
