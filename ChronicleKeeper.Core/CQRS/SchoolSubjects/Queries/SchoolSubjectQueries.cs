using ChronicleKeeper.Core.DTOs.SchoolSubject;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.SchoolSubjects.Queries
{
    public class GetAllSchoolSubjectsQuery : IRequest<List<SchoolSubjectDto>>
    {
        public int? WorldId { get; set; }
        public int? SchoolId { get; set; }
    }

    public class GetSchoolSubjectByIdQuery : IRequest<SchoolSubjectDetailsDto?>
    {
        public int Id { get; set; }
    }
}
