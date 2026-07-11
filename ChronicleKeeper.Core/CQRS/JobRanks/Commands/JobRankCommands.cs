using ChronicleKeeper.Core.DTOs.JobRank;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.JobRanks.Commands
{
    public class CreateJobRankCommand : IRequest<JobRankDto>
    {
        public JobRankCreateDto JobRankCreateDto { get; set; } = new();
    }

    public class UpdateJobRankCommand : IRequest<JobRankDto>
    {
        public int Id { get; set; }
        public JobRankUpdateDto JobRankUpdateDto { get; set; } = new();
    }

    public class DeleteJobRankCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
