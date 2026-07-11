using ChronicleKeeper.Core.DTOs.Specialisation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Specialisations.Commands
{
    public class CreateSpecialisationCommand : IRequest<SpecialisationDto>
    {
        public SpecialisationCreateDto SpecialisationCreateDto { get; set; } = new();
    }

    public class UpdateSpecialisationCommand : IRequest<SpecialisationDto>
    {
        public int Id { get; set; }
        public SpecialisationUpdateDto SpecialisationUpdateDto { get; set; } = new();
    }

    public class DeleteSpecialisationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
