using ChronicleKeeper.Core.DTOs.Mythology;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mythology
{
    public class GetAllReligiousOrdersQuery : IRequest<List<ReligiousOrderDto>> { public int? WorldId { get; set; } }
    public class GetReligiousOrderByIdQuery : IRequest<ReligiousOrderDetailsDto?> { public int Id { get; set; } }

    public class GetAllDeitiesQuery : IRequest<List<DeityDto>> { public int? WorldId { get; set; } }
    public class GetDeityByIdQuery : IRequest<DeityDetailsDto?> { public int Id { get; set; } }
}
