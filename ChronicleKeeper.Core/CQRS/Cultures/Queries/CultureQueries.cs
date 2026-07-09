using ChronicleKeeper.Core.DTOs.Culture;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Cultures.Queries
{
    public class GetAllCulturesQuery : IRequest<List<CultureDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetCultureByIdQuery : IRequest<CultureDetailsDto?>
    {
        public int Id { get; set; }
    }
}
