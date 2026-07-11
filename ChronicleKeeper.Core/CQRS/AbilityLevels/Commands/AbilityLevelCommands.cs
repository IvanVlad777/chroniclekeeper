using ChronicleKeeper.Core.DTOs.AbilityLevel;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.AbilityLevels.Commands
{
    public class CreateAbilityLevelCommand : IRequest<AbilityLevelDto>
    {
        public AbilityLevelCreateDto AbilityLevelCreateDto { get; set; } = new();
    }

    public class UpdateAbilityLevelCommand : IRequest<AbilityLevelDto>
    {
        public int Id { get; set; }
        public AbilityLevelUpdateDto AbilityLevelUpdateDto { get; set; } = new();
    }

    public class DeleteAbilityLevelCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
