using ChronicleKeeper.Core.DTOs.Religion;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Religions.Queries
{
    public class GetAllReligionsQuery : IRequest<List<ReligionDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetReligionByIdQuery : IRequest<ReligionDetailsDto?>
    {
        public int Id { get; set; }
    }
}
