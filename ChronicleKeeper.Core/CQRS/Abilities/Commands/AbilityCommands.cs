using ChronicleKeeper.Core.DTOs.Ability;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Abilities.Commands
{
    public class CreateAbilityCommand : IRequest<AbilityDto>
    {
        public AbilityCreateDto AbilityCreateDto { get; set; } = new();
    }

    public class UpdateAbilityCommand : IRequest<AbilityDto>
    {
        public int Id { get; set; }
        public AbilityUpdateDto AbilityUpdateDto { get; set; } = new();
    }

    public class DeleteAbilityCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
