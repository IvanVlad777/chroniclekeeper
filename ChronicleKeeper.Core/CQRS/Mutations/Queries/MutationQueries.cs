using ChronicleKeeper.Core.DTOs.Mutation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mutations.Queries
{
    public class GetAllMutationsQuery : IRequest<List<MutationDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetMutationByIdQuery : IRequest<MutationDetailsDto?>
    {
        public int Id { get; set; }
    }
}
