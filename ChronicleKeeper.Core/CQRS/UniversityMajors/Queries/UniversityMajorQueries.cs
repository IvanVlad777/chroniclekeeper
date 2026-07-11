using ChronicleKeeper.Core.DTOs.UniversityMajor;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.UniversityMajors.Queries
{
    public class GetAllUniversityMajorsQuery : IRequest<List<UniversityMajorDto>>
    {
        public int? WorldId { get; set; }
        public int? UniversityId { get; set; }
    }

    public class GetUniversityMajorByIdQuery : IRequest<UniversityMajorDto?>
    {
        public int Id { get; set; }
    }
}
