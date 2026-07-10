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

    public class AddPoliticalPartyFactionCommand : IRequest<bool>
    {
        public int PoliticalPartyId { get; set; }
        public int FactionId { get; set; }
    }

    public class RemovePoliticalPartyFactionCommand : IRequest<bool>
    {
        public int PoliticalPartyId { get; set; }
        public int FactionId { get; set; }
    }

    public class AddPoliticalPartyNationCommand : IRequest<bool>
    {
        public int PoliticalPartyId { get; set; }
        public int NationId { get; set; }
    }

    public class RemovePoliticalPartyNationCommand : IRequest<bool>
    {
        public int PoliticalPartyId { get; set; }
        public int NationId { get; set; }
    }
}
