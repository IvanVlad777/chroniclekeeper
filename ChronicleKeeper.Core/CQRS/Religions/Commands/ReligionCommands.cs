using ChronicleKeeper.Core.DTOs.Religion;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Religions.Commands
{
    public class CreateReligionCommand : IRequest<ReligionDto>
    {
        public ReligionCreateDto ReligionCreateDto { get; set; } = new();
    }

    public class UpdateReligionCommand : IRequest<ReligionDto>
    {
        public int Id { get; set; }
        public ReligionUpdateDto ReligionUpdateDto { get; set; } = new();
    }

    public class DeleteReligionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
