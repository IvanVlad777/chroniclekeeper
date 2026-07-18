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

    public class AddUniversityStudentCommand : IRequest<bool>
    {
        public int UniversityId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveUniversityStudentCommand : IRequest<bool>
    {
        public int UniversityId { get; set; }
        public int CharacterId { get; set; }
    }

    public class AddUniversityProfessorCommand : IRequest<bool>
    {
        public int UniversityId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveUniversityProfessorCommand : IRequest<bool>
    {
        public int UniversityId { get; set; }
        public int CharacterId { get; set; }
    }
}
