using ChronicleKeeper.Core.DTOs.ReligiousEducation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ReligiousEducations.Queries
{
    public class GetAllReligiousEducationsQuery : IRequest<List<ReligiousEducationDto>>
    {
        public int? WorldId { get; set; }
        public int? CharacterId { get; set; }
        public int? ReligionId { get; set; }
    }

    public class GetReligiousEducationByIdQuery : IRequest<ReligiousEducationDto?>
    {
        public int Id { get; set; }
    }
}
