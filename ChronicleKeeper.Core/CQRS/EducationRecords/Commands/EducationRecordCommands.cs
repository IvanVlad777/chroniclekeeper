using ChronicleKeeper.Core.DTOs.EducationRecord;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.EducationRecords.Commands
{
    public class CreateEducationRecordCommand : IRequest<EducationRecordDto>
    {
        public EducationRecordCreateDto EducationRecordCreateDto { get; set; } = new();
    }

    public class UpdateEducationRecordCommand : IRequest<EducationRecordDto>
    {
        public int Id { get; set; }
        public EducationRecordUpdateDto EducationRecordUpdateDto { get; set; } = new();
    }

    public class DeleteEducationRecordCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
