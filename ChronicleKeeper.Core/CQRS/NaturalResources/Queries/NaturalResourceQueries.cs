using ChronicleKeeper.Core.DTOs.NaturalResource;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.NaturalResources.Queries
{
    public class GetAllNaturalResourcesQuery : IRequest<List<NaturalResourceDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetNaturalResourceByIdQuery : IRequest<NaturalResourceDetailsDto?>
    {
        public int Id { get; set; }
    }
}
