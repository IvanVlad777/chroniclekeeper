using ChronicleKeeper.Core.DTOs.UniversityMajor;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.UniversityMajors.Commands
{
    public class CreateUniversityMajorCommand : IRequest<UniversityMajorDto>
    {
        public UniversityMajorCreateDto UniversityMajorCreateDto { get; set; } = new();
    }

    public class UpdateUniversityMajorCommand : IRequest<UniversityMajorDto>
    {
        public int Id { get; set; }
        public UniversityMajorUpdateDto UniversityMajorUpdateDto { get; set; } = new();
    }

    public class DeleteUniversityMajorCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddUniversityMajorProfessorCommand : IRequest<bool>
    {
        public int UniversityMajorId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveUniversityMajorProfessorCommand : IRequest<bool>
    {
        public int UniversityMajorId { get; set; }
        public int CharacterId { get; set; }
    }

    public class AddUniversityMajorStudentCommand : IRequest<bool>
    {
        public int UniversityMajorId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveUniversityMajorStudentCommand : IRequest<bool>
    {
        public int UniversityMajorId { get; set; }
        public int CharacterId { get; set; }
    }
}
