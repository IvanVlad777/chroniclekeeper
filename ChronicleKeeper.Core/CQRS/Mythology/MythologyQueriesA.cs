using ChronicleKeeper.Core.DTOs.Mythology;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mythology
{
    public class GetAllHolySitesQuery : IRequest<List<HolySiteDto>> { public int? WorldId { get; set; } }
    public class GetHolySiteByIdQuery : IRequest<HolySiteDetailsDto?> { public int Id { get; set; } }

    public class GetAllReligiousTextsQuery : IRequest<List<ReligiousTextDto>> { public int? WorldId { get; set; } }
    public class GetReligiousTextByIdQuery : IRequest<ReligiousTextDetailsDto?> { public int Id { get; set; } }

    public class GetAllReligiousFestivalsQuery : IRequest<List<ReligiousFestivalDto>> { public int? WorldId { get; set; } }
    public class GetReligiousFestivalByIdQuery : IRequest<ReligiousFestivalDetailsDto?> { public int Id { get; set; } }
}
