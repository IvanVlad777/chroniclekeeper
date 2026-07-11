using ChronicleKeeper.Core.DTOs.Specialisation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Specialisations.Queries
{
    public class GetAllSpecialisationsQuery : IRequest<List<SpecialisationDto>>
    {
        public int? WorldId { get; set; }
        public int? ProfessionId { get; set; }
    }

    public class GetSpecialisationByIdQuery : IRequest<SpecialisationDto?>
    {
        public int Id { get; set; }
    }
}
