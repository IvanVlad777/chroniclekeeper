using ChronicleKeeper.Core.DTOs.Profession;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Professions.Commands
{
    public class CreateProfessionCommand : IRequest<ProfessionDto>
    {
        public ProfessionCreateDto ProfessionCreateDto { get; set; } = new();
    }

    public class UpdateProfessionCommand : IRequest<ProfessionDto>
    {
        public int Id { get; set; }
        public ProfessionUpdateDto ProfessionUpdateDto { get; set; } = new();
    }

    public class DeleteProfessionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddProfessionSpeciesCommand : IRequest<bool>
    {
        public int ProfessionId { get; set; }
        public int SpeciesId { get; set; }
    }

    public class RemoveProfessionSpeciesCommand : IRequest<bool>
    {
        public int ProfessionId { get; set; }
        public int SpeciesId { get; set; }
    }

    public class AddProfessionSocialClassCommand : IRequest<bool>
    {
        public int ProfessionId { get; set; }
        public int SocialClassId { get; set; }
    }

    public class RemoveProfessionSocialClassCommand : IRequest<bool>
    {
        public int ProfessionId { get; set; }
        public int SocialClassId { get; set; }
    }

    public class AddProfessionTradeSchoolCommand : IRequest<bool>
    {
        public int ProfessionId { get; set; }
        public int TradeSchoolId { get; set; }
    }

    public class RemoveProfessionTradeSchoolCommand : IRequest<bool>
    {
        public int ProfessionId { get; set; }
        public int TradeSchoolId { get; set; }
    }
}
