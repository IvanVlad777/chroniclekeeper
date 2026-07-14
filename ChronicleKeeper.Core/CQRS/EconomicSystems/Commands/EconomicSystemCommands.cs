using ChronicleKeeper.Core.DTOs.EconomicSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.EconomicSystems.Commands
{
    public class CreateEconomicSystemCommand : IRequest<EconomicSystemDto>
    {
        public EconomicSystemCreateDto EconomicSystemCreateDto { get; set; } = new();
    }

    public class UpdateEconomicSystemCommand : IRequest<EconomicSystemDto>
    {
        public int Id { get; set; }
        public EconomicSystemUpdateDto EconomicSystemUpdateDto { get; set; } = new();
    }

    public class DeleteEconomicSystemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
