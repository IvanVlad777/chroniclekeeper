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
}
