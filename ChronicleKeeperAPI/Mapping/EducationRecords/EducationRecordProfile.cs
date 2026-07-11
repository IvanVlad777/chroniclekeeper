using AutoMapper;
using ChronicleKeeper.Core.DTOs.EducationRecord;
using ChronicleKeeper.Core.Entities.Social.Education;

public class EducationRecordProfile : Profile
{
    public EducationRecordProfile()
    {
        CreateMap<EducationRecord, EducationRecordDto>();
        CreateMap<EducationRecordCreateDto, EducationRecord>();
        CreateMap<EducationRecordUpdateDto, EducationRecord>();
    }
}
