using AutoMapper;
using ChronicleKeeper.Core.DTOs.JobRank;
using ChronicleKeeper.Core.Entities.Professions;

public class JobRankProfile : Profile
{
    public JobRankProfile()
    {
        CreateMap<JobRank, JobRankDto>();
        CreateMap<JobRankCreateDto, JobRank>();
        CreateMap<JobRankUpdateDto, JobRank>();
    }
}
