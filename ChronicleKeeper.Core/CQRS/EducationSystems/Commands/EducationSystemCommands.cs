using ChronicleKeeper.Core.DTOs.EducationSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.EducationSystems.Commands
{
    public class CreateEducationSystemCommand : IRequest<EducationSystemDto>
    {
        public EducationSystemCreateDto EducationSystemCreateDto { get; set; } = new();
    }

    public class UpdateEducationSystemCommand : IRequest<EducationSystemDto>
    {
        public int Id { get; set; }
        public EducationSystemUpdateDto EducationSystemUpdateDto { get; set; } = new();
    }

    public class DeleteEducationSystemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
