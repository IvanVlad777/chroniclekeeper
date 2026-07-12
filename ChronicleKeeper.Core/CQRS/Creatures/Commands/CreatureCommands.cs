using ChronicleKeeper.Core.DTOs.Creature;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Creatures.Commands
{
    public class CreateCreatureCommand : IRequest<CreatureDto>
    {
        public CreatureCreateDto CreatureCreateDto { get; set; } = new();
    }

    public class UpdateCreatureCommand : IRequest<CreatureDto>
    {
        public int Id { get; set; }
        public CreatureUpdateDto CreatureUpdateDto { get; set; } = new();
    }

    public class DeleteCreatureCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddCreatureCityCommand : IRequest<bool>
    {
        public int CreatureId { get; set; }
        public int CityId { get; set; }
    }

    public class RemoveCreatureCityCommand : IRequest<bool>
    {
        public int CreatureId { get; set; }
        public int CityId { get; set; }
    }

    public class AddCreatureHabitatCommand : IRequest<bool>
    {
        public int CreatureId { get; set; }
        public int EcosystemId { get; set; }
    }

    public class RemoveCreatureHabitatCommand : IRequest<bool>
    {
        public int CreatureId { get; set; }
        public int EcosystemId { get; set; }
    }
}
