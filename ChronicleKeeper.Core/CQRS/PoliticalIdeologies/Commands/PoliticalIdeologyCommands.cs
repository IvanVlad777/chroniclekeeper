using ChronicleKeeper.Core.DTOs.PoliticalIdeology;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.PoliticalIdeologies.Commands
{
    public class CreatePoliticalIdeologyCommand : IRequest<PoliticalIdeologyDto>
    {
        public PoliticalIdeologyCreateDto PoliticalIdeologyCreateDto { get; set; } = new();
    }

    public class UpdatePoliticalIdeologyCommand : IRequest<PoliticalIdeologyDto>
    {
        public int Id { get; set; }
        public PoliticalIdeologyUpdateDto PoliticalIdeologyUpdateDto { get; set; } = new();
    }

    public class DeletePoliticalIdeologyCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
