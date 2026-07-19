using ChronicleKeeper.Core.DTOs.Location;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Locations.Commands
{
    public class CreateLocationCommand : IRequest<LocationDto>
    {
        public LocationCreateDto LocationCreateDto { get; set; } = new();
    }

    public class UpdateLocationCommand : IRequest<LocationDto>
    {
        public int Id { get; set; }
        public LocationUpdateDto LocationUpdateDto { get; set; } = new();
    }

    public class DeleteLocationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddRegionNativeSpeciesCommand : IRequest<bool>
    {
        public int RegionId { get; set; }
        public int SapientSpeciesId { get; set; }
    }

    public class RemoveRegionNativeSpeciesCommand : IRequest<bool>
    {
        public int RegionId { get; set; }
        public int SapientSpeciesId { get; set; }
    }

    public class AddLocationCrossLinkCommand : IRequest<bool>
    {
        public int LocationId { get; set; }
        public LocationLinkTargetType TargetType { get; set; }
        public int TargetId { get; set; }
    }

    public class RemoveLocationCrossLinkCommand : IRequest<bool>
    {
        public int LocationId { get; set; }
        public LocationLinkTargetType TargetType { get; set; }
        public int TargetId { get; set; }
    }
}
