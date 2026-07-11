using ChronicleKeeper.Core.DTOs.Content;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Contents.Commands
{
    public class CreateContentCommand : IRequest<ContentDto>
    {
        public ContentCreateDto ContentCreateDto { get; set; } = new();
    }

    public class UpdateContentCommand : IRequest<ContentDto>
    {
        public int Id { get; set; }
        public ContentUpdateDto ContentUpdateDto { get; set; } = new();
    }

    public class DeleteContentCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
