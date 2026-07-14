using ChronicleKeeper.Core.DTOs.Industry;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Industries.Queries
{
    public class GetAllIndustriesQuery : IRequest<List<IndustryDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetIndustryByIdQuery : IRequest<IndustryDetailsDto?>
    {
        public int Id { get; set; }
    }
}
