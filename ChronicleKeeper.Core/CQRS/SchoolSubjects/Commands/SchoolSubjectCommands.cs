using ChronicleKeeper.Core.DTOs.SchoolSubject;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.SchoolSubjects.Commands
{
    public class CreateSchoolSubjectCommand : IRequest<SchoolSubjectDto>
    {
        public SchoolSubjectCreateDto SchoolSubjectCreateDto { get; set; } = new();
    }

    public class UpdateSchoolSubjectCommand : IRequest<SchoolSubjectDto>
    {
        public int Id { get; set; }
        public SchoolSubjectUpdateDto SchoolSubjectUpdateDto { get; set; } = new();
    }

    public class DeleteSchoolSubjectCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
