using ChronicleKeeper.Core.DTOs.PoliticalParty;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.PoliticalParties.Commands
{
    public class CreatePoliticalPartyCommand : IRequest<PoliticalPartyDto>
    {
        public PoliticalPartyCreateDto PoliticalPartyCreateDto { get; set; } = new();
    }

    public class UpdatePoliticalPartyCommand : IRequest<PoliticalPartyDto>
    {
        public int Id { get; set; }
        public PoliticalPartyUpdateDto PoliticalPartyUpdateDto { get; set; } = new();
    }

    public class DeletePoliticalPartyCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
