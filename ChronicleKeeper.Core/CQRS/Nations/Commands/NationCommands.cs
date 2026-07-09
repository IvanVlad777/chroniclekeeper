using ChronicleKeeper.Core.DTOs.Nation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Nations.Commands
{
    public class CreateNationCommand : IRequest<NationDto>
    {
        public NationCreateDto NationCreateDto { get; set; } = new();
    }

    public class UpdateNationCommand : IRequest<NationDto>
    {
        public int Id { get; set; }
        public NationUpdateDto NationUpdateDto { get; set; } = new();
    }

    public class DeleteNationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
