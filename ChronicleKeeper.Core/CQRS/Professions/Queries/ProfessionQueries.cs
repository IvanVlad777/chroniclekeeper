using ChronicleKeeper.Core.DTOs.Profession;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Professions.Queries
{
    public class GetAllProfessionsQuery : IRequest<List<ProfessionDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetProfessionByIdQuery : IRequest<ProfessionDetailsDto?>
    {
        public int Id { get; set; }
    }
}
