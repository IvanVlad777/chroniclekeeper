using ChronicleKeeper.Core.DTOs.LegalSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.LegalSystems.Commands
{
    public class CreateLegalSystemCommand : IRequest<LegalSystemDto>
    {
        public LegalSystemCreateDto LegalSystemCreateDto { get; set; } = new();
    }

    public class UpdateLegalSystemCommand : IRequest<LegalSystemDto>
    {
        public int Id { get; set; }
        public LegalSystemUpdateDto LegalSystemUpdateDto { get; set; } = new();
    }

    public class DeleteLegalSystemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
