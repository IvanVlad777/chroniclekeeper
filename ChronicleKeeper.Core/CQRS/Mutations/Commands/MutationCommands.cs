using ChronicleKeeper.Core.DTOs.Mutation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mutations.Commands
{
    public class CreateMutationCommand : IRequest<MutationDto>
    {
        public MutationCreateDto MutationCreateDto { get; set; } = new();
    }

    public class UpdateMutationCommand : IRequest<MutationDto>
    {
        public int Id { get; set; }
        public MutationUpdateDto MutationUpdateDto { get; set; } = new();
    }

    public class DeleteMutationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
