using ChronicleKeeper.Core.DTOs.Hobby;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Hobbies.Commands
{
    public class CreateHobbyCommand : IRequest<HobbyDto>
    {
        public HobbyCreateDto HobbyCreateDto { get; set; } = new();
    }

    public class UpdateHobbyCommand : IRequest<HobbyDto>
    {
        public int Id { get; set; }
        public HobbyUpdateDto HobbyUpdateDto { get; set; } = new();
    }

    public class DeleteHobbyCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
