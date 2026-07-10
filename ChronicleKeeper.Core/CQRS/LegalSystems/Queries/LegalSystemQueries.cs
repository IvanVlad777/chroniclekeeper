using ChronicleKeeper.Core.DTOs.LegalSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.LegalSystems.Queries
{
    public class GetAllLegalSystemsQuery : IRequest<List<LegalSystemDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetLegalSystemByIdQuery : IRequest<LegalSystemDetailsDto?>
    {
        public int Id { get; set; }
    }
}
