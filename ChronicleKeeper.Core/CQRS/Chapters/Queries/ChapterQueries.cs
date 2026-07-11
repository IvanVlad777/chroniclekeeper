using ChronicleKeeper.Core.DTOs.Chapter;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Chapters.Queries
{
    public class GetAllChaptersQuery : IRequest<List<ChapterDto>>
    {
        public int? WorldId { get; set; }
        public int? BookId { get; set; }
    }

    public class GetChapterByIdQuery : IRequest<ChapterDto?>
    {
        public int Id { get; set; }
    }
}
