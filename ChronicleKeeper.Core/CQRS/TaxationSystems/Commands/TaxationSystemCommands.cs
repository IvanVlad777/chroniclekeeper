using ChronicleKeeper.Core.DTOs.TaxationSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.TaxationSystems.Commands
{
    public class CreateTaxationSystemCommand : IRequest<TaxationSystemDto>
    {
        public TaxationSystemCreateDto TaxationSystemCreateDto { get; set; } = new();
    }

    public class UpdateTaxationSystemCommand : IRequest<TaxationSystemDto>
    {
        public int Id { get; set; }
        public TaxationSystemUpdateDto TaxationSystemUpdateDto { get; set; } = new();
    }

    public class DeleteTaxationSystemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
