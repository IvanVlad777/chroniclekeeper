using ChronicleKeeper.Core.DTOs.Content;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Contents.Queries
{
    public class GetAllContentsQuery : IRequest<List<ContentDto>>
    {
        public int? WorldId { get; set; }

        /// <summary>Optional exact-match filter on the TPH discriminator ("Article"/"Book"/"Comic"/"Movie"/"Series").</summary>
        public string? Type { get; set; }
    }

    public class GetContentByIdQuery : IRequest<ContentDetailsDto?>
    {
        public int Id { get; set; }
    }
}
