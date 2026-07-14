using ChronicleKeeper.Core.DTOs.ExtractionMethod;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ExtractionMethods.Queries
{
    public class GetAllExtractionMethodsQuery : IRequest<List<ExtractionMethodDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetExtractionMethodByIdQuery : IRequest<ExtractionMethodDetailsDto?>
    {
        public int Id { get; set; }
    }
}
