using ChronicleKeeper.Core.DTOs.Corporation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Corporations.Commands
{
    public class CreateCorporationCommand : IRequest<CorporationDto>
    {
        public CorporationCreateDto CorporationCreateDto { get; set; } = new();
    }

    public class UpdateCorporationCommand : IRequest<CorporationDto>
    {
        public int Id { get; set; }
        public CorporationUpdateDto CorporationUpdateDto { get; set; } = new();
    }

    public class DeleteCorporationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddCorporationFactionCommand : IRequest<bool>
    {
        public int CorporationId { get; set; }
        public int FactionId { get; set; }
    }

    public class RemoveCorporationFactionCommand : IRequest<bool>
    {
        public int CorporationId { get; set; }
        public int FactionId { get; set; }
    }

    public class AddCorporationProfessionCommand : IRequest<bool>
    {
        public int CorporationId { get; set; }
        public int ProfessionId { get; set; }
    }

    public class RemoveCorporationProfessionCommand : IRequest<bool>
    {
        public int CorporationId { get; set; }
        public int ProfessionId { get; set; }
    }
}
