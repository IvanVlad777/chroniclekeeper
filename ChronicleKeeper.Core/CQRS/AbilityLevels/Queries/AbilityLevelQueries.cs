using ChronicleKeeper.Core.DTOs.AbilityLevel;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.AbilityLevels.Queries
{
    public class GetAllAbilityLevelsQuery : IRequest<List<AbilityLevelDto>>
    {
        public int? WorldId { get; set; }
        public int? AbilityId { get; set; }
    }

    public class GetAbilityLevelByIdQuery : IRequest<AbilityLevelDto?>
    {
        public int Id { get; set; }
    }
}
