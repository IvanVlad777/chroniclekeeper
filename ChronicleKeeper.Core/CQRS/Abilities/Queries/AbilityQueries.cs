using ChronicleKeeper.Core.DTOs.Ability;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Abilities.Queries
{
    public class GetAllAbilitiesQuery : IRequest<List<AbilityDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetAbilityByIdQuery : IRequest<AbilityDto?>
    {
        public int Id { get; set; }
    }
}
