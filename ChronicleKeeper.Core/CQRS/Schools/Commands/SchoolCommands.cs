using ChronicleKeeper.Core.DTOs.School;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Schools.Commands
{
    public class CreateSchoolCommand : IRequest<SchoolDto>
    {
        public SchoolCreateDto SchoolCreateDto { get; set; } = new();
    }

    public class UpdateSchoolCommand : IRequest<SchoolDto>
    {
        public int Id { get; set; }
        public SchoolUpdateDto SchoolUpdateDto { get; set; } = new();
    }

    public class DeleteSchoolCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddSchoolStudentCommand : IRequest<bool>
    {
        public int SchoolId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveSchoolStudentCommand : IRequest<bool>
    {
        public int SchoolId { get; set; }
        public int CharacterId { get; set; }
    }

    public class AddSchoolTeacherCommand : IRequest<bool>
    {
        public int SchoolId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveSchoolTeacherCommand : IRequest<bool>
    {
        public int SchoolId { get; set; }
        public int CharacterId { get; set; }
    }
}
