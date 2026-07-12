using ChronicleKeeper.Core.DTOs.Creature;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Creatures.Queries
{
    public class GetAllCreaturesQuery : IRequest<List<CreatureDto>>
    {
        public int? WorldId { get; set; }

        /// <summary>Optional exact-match filter on the TPH discriminator ("Animal"/"Plant"/"Tree"/"Crop"/"Fungus").</summary>
        public string? Subtype { get; set; }
    }

    public class GetCreatureByIdQuery : IRequest<CreatureDetailsDto?>
    {
        public int Id { get; set; }
    }
}
