using ChronicleKeeper.Core.DTOs.ClimateZone;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ClimateZones.Commands
{
    public class CreateClimateZoneCommand : IRequest<ClimateZoneDto>
    {
        public ClimateZoneCreateDto ClimateZoneCreateDto { get; set; } = new();
    }

    public class UpdateClimateZoneCommand : IRequest<ClimateZoneDto>
    {
        public int Id { get; set; }
        public ClimateZoneUpdateDto ClimateZoneUpdateDto { get; set; } = new();
    }

    public class DeleteClimateZoneCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddClimateZoneDetailCommand : IRequest<bool>
    {
        public int ClimateZoneId { get; set; }
        public int ClimateDetailId { get; set; }
    }

    public class RemoveClimateZoneDetailCommand : IRequest<bool>
    {
        public int ClimateZoneId { get; set; }
        public int ClimateDetailId { get; set; }
    }

    public class AddClimateZoneSeasonCommand : IRequest<bool>
    {
        public int ClimateZoneId { get; set; }
        public int SeasonId { get; set; }
    }

    public class RemoveClimateZoneSeasonCommand : IRequest<bool>
    {
        public int ClimateZoneId { get; set; }
        public int SeasonId { get; set; }
    }

    public class AddClimateZoneLocationCommand : IRequest<bool>
    {
        public int ClimateZoneId { get; set; }
        public int LocationId { get; set; }
    }

    public class RemoveClimateZoneLocationCommand : IRequest<bool>
    {
        public int ClimateZoneId { get; set; }
        public int LocationId { get; set; }
    }
}
