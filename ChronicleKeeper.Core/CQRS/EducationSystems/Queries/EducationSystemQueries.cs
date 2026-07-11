using ChronicleKeeper.Core.DTOs.EducationSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.EducationSystems.Queries
{
    public class GetAllEducationSystemsQuery : IRequest<List<EducationSystemDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetEducationSystemByIdQuery : IRequest<EducationSystemDetailsDto?>
    {
        public int Id { get; set; }
    }
}
