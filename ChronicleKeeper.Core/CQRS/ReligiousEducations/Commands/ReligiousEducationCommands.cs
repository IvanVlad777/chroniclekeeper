using ChronicleKeeper.Core.DTOs.ReligiousEducation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ReligiousEducations.Commands
{
    public class CreateReligiousEducationCommand : IRequest<ReligiousEducationDto>
    {
        public ReligiousEducationCreateDto ReligiousEducationCreateDto { get; set; } = new();
    }

    public class UpdateReligiousEducationCommand : IRequest<ReligiousEducationDto>
    {
        public int Id { get; set; }
        public ReligiousEducationUpdateDto ReligiousEducationUpdateDto { get; set; } = new();
    }

    public class DeleteReligiousEducationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
