using ChronicleKeeper.Core.DTOs.EducationRecord;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.EducationRecords.Queries
{
    public class GetAllEducationRecordsQuery : IRequest<List<EducationRecordDto>>
    {
        public int? WorldId { get; set; }
        public int? CharacterId { get; set; }
        public int? SchoolId { get; set; }
        public int? UniversityId { get; set; }
    }

    public class GetEducationRecordByIdQuery : IRequest<EducationRecordDto?>
    {
        public int Id { get; set; }
    }
}
