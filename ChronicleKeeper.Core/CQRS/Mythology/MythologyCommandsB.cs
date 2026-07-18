using ChronicleKeeper.Core.DTOs.Mythology;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mythology
{
    // ReligiousOrder
    public class CreateReligiousOrderCommand : IRequest<ReligiousOrderDto> { public ReligiousOrderCreateDto Dto { get; set; } = new(); }
    public class UpdateReligiousOrderCommand : IRequest<ReligiousOrderDto> { public int Id { get; set; } public ReligiousOrderUpdateDto Dto { get; set; } = new(); }
    public class DeleteReligiousOrderCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddReligiousOrderFactionCommand : IRequest<Unit> { public int OrderId { get; set; } public int FactionId { get; set; } }
    public class RemoveReligiousOrderFactionCommand : IRequest<Unit> { public int OrderId { get; set; } public int FactionId { get; set; } }

    // Deity
    public class CreateDeityCommand : IRequest<DeityDto> { public DeityCreateDto Dto { get; set; } = new(); }
    public class UpdateDeityCommand : IRequest<DeityDto> { public int Id { get; set; } public DeityUpdateDto Dto { get; set; } = new(); }
    public class DeleteDeityCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddDeityOrderCommand : IRequest<Unit> { public int DeityId { get; set; } public int OrderId { get; set; } }
    public class RemoveDeityOrderCommand : IRequest<Unit> { public int DeityId { get; set; } public int OrderId { get; set; } }
    public class AddDeityAllyCommand : IRequest<Unit> { public int DeityId { get; set; } public int AlliedDeityId { get; set; } }
    public class RemoveDeityAllyCommand : IRequest<Unit> { public int DeityId { get; set; } public int AlliedDeityId { get; set; } }
    public class AddDeityRivalCommand : IRequest<Unit> { public int DeityId { get; set; } public int RivalDeityId { get; set; } }
    public class RemoveDeityRivalCommand : IRequest<Unit> { public int DeityId { get; set; } public int RivalDeityId { get; set; } }
}
