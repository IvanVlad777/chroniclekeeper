using ChronicleKeeper.Core.DTOs.Nation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Nations.Queries
{
    public class GetAllNationsQuery : IRequest<List<NationDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetNationByIdQuery : IRequest<NationDetailsDto?>
    {
        public int Id { get; set; }
    }
}
