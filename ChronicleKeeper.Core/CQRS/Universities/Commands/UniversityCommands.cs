using ChronicleKeeper.Core.DTOs.University;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Universities.Commands
{
    public class CreateUniversityCommand : IRequest<UniversityDto>
    {
        public UniversityCreateDto UniversityCreateDto { get; set; } = new();
    }

    public class UpdateUniversityCommand : IRequest<UniversityDto>
    {
        public int Id { get; set; }
        public UniversityUpdateDto UniversityUpdateDto { get; set; } = new();
    }

    public class DeleteUniversityCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
