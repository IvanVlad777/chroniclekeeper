using ChronicleKeeper.Core.DTOs.Library;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Libraries.Commands
{
    public class CreateLibraryCommand : IRequest<LibraryDto>
    {
        public LibraryCreateDto LibraryCreateDto { get; set; } = new();
    }

    public class UpdateLibraryCommand : IRequest<LibraryDto>
    {
        public int Id { get; set; }
        public LibraryUpdateDto LibraryUpdateDto { get; set; } = new();
    }

    public class DeleteLibraryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddLibraryScholarCommand : IRequest<bool>
    {
        public int LibraryId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveLibraryScholarCommand : IRequest<bool>
    {
        public int LibraryId { get; set; }
        public int CharacterId { get; set; }
    }
}
