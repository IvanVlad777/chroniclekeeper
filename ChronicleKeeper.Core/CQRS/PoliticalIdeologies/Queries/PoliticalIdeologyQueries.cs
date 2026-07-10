using ChronicleKeeper.Core.DTOs.PoliticalIdeology;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.PoliticalIdeologies.Queries
{
    public class GetAllPoliticalIdeologiesQuery : IRequest<List<PoliticalIdeologyDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetPoliticalIdeologyByIdQuery : IRequest<PoliticalIdeologyDetailsDto?>
    {
        public int Id { get; set; }
    }
}
