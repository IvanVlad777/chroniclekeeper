using ChronicleKeeper.Core.DTOs.ClimateDetail;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ClimateDetails.Commands
{
    public class CreateClimateDetailCommand : IRequest<ClimateDetailDto>
    {
        public ClimateDetailCreateDto ClimateDetailCreateDto { get; set; } = new();
    }

    public class UpdateClimateDetailCommand : IRequest<ClimateDetailDto>
    {
        public int Id { get; set; }
        public ClimateDetailUpdateDto ClimateDetailUpdateDto { get; set; } = new();
    }

    public class DeleteClimateDetailCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
