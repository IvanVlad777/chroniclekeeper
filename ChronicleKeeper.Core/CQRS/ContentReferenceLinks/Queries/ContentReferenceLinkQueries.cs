using ChronicleKeeper.Core.DTOs.ContentReferenceLink;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ContentReferenceLinks.Queries
{
    public class GetAllContentReferenceLinksQuery : IRequest<List<ContentReferenceLinkDto>>
    {
        public int? ContentId { get; set; }
        public int? ChapterId { get; set; }
        public int? EpisodeId { get; set; }
        public int? CharacterId { get; set; }
        public int? LocationId { get; set; }
        public int? FactionId { get; set; }
        public int? NationId { get; set; }
    }

    public class GetContentReferenceLinkByIdQuery : IRequest<ContentReferenceLinkDto?>
    {
        public int Id { get; set; }
    }
}
