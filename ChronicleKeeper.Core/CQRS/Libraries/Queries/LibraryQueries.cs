using ChronicleKeeper.Core.DTOs.Library;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Libraries.Queries
{
    public class GetAllLibrariesQuery : IRequest<List<LibraryDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetLibraryByIdQuery : IRequest<LibraryDetailsDto?>
    {
        public int Id { get; set; }
    }
}
