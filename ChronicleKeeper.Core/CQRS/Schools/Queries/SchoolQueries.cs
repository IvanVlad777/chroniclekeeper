using ChronicleKeeper.Core.DTOs.School;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Schools.Queries
{
    public class GetAllSchoolsQuery : IRequest<List<SchoolDto>>
    {
        public int? WorldId { get; set; }
        public int? EducationSystemId { get; set; }
    }

    public class GetSchoolByIdQuery : IRequest<SchoolDetailsDto?>
    {
        public int Id { get; set; }
    }
}
