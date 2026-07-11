using ChronicleKeeper.Core.DTOs.JobRank;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.JobRanks.Queries
{
    public class GetAllJobRanksQuery : IRequest<List<JobRankDto>>
    {
        public int? WorldId { get; set; }
        public int? ProfessionId { get; set; }
    }

    public class GetJobRankByIdQuery : IRequest<JobRankDto?>
    {
        public int Id { get; set; }
    }
}
