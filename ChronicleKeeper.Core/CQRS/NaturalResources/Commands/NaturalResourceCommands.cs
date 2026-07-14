using ChronicleKeeper.Core.DTOs.NaturalResource;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.NaturalResources.Commands
{
    public class CreateNaturalResourceCommand : IRequest<NaturalResourceDto>
    {
        public NaturalResourceCreateDto NaturalResourceCreateDto { get; set; } = new();
    }

    public class UpdateNaturalResourceCommand : IRequest<NaturalResourceDto>
    {
        public int Id { get; set; }
        public NaturalResourceUpdateDto NaturalResourceUpdateDto { get; set; } = new();
    }

    public class DeleteNaturalResourceCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddNaturalResourceLocationCommand : IRequest<bool>
    {
        public int NaturalResourceId { get; set; }
        public int LocationId { get; set; }
    }

    public class RemoveNaturalResourceLocationCommand : IRequest<bool>
    {
        public int NaturalResourceId { get; set; }
        public int LocationId { get; set; }
    }
}
