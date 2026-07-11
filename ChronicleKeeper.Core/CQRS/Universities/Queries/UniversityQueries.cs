using ChronicleKeeper.Core.DTOs.University;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Universities.Queries
{
    public class GetAllUniversitiesQuery : IRequest<List<UniversityDto>>
    {
        public int? WorldId { get; set; }
        public int? EducationSystemId { get; set; }
    }

    public class GetUniversityByIdQuery : IRequest<UniversityDetailsDto?>
    {
        public int Id { get; set; }
    }
}
