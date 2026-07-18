using ChronicleKeeper.Core.DTOs.Mythology;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mythology
{
    // HolySite
    public class CreateHolySiteCommand : IRequest<HolySiteDto> { public HolySiteCreateDto Dto { get; set; } = new(); }
    public class UpdateHolySiteCommand : IRequest<HolySiteDto> { public int Id { get; set; } public HolySiteUpdateDto Dto { get; set; } = new(); }
    public class DeleteHolySiteCommand : IRequest<bool> { public int Id { get; set; } }

    // ReligiousText
    public class CreateReligiousTextCommand : IRequest<ReligiousTextDto> { public ReligiousTextCreateDto Dto { get; set; } = new(); }
    public class UpdateReligiousTextCommand : IRequest<ReligiousTextDto> { public int Id { get; set; } public ReligiousTextUpdateDto Dto { get; set; } = new(); }
    public class DeleteReligiousTextCommand : IRequest<bool> { public int Id { get; set; } }

    // ReligiousFestival
    public class CreateReligiousFestivalCommand : IRequest<ReligiousFestivalDto> { public ReligiousFestivalCreateDto Dto { get; set; } = new(); }
    public class UpdateReligiousFestivalCommand : IRequest<ReligiousFestivalDto> { public int Id { get; set; } public ReligiousFestivalUpdateDto Dto { get; set; } = new(); }
    public class DeleteReligiousFestivalCommand : IRequest<bool> { public int Id { get; set; } }
}
