using ChronicleKeeper.Core.DTOs.Chapter;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Chapters.Commands
{
    public class CreateChapterCommand : IRequest<ChapterDto>
    {
        public ChapterCreateDto ChapterCreateDto { get; set; } = new();
    }

    public class UpdateChapterCommand : IRequest<ChapterDto>
    {
        public int Id { get; set; }
        public ChapterUpdateDto ChapterUpdateDto { get; set; } = new();
    }

    public class DeleteChapterCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
