using ChronicleKeeper.Core.DTOs.Culture;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Cultures.Commands
{
    public class CreateCultureCommand : IRequest<CultureDto>
    {
        public CultureCreateDto CultureCreateDto { get; set; } = new();
    }

    public class UpdateCultureCommand : IRequest<CultureDto>
    {
        public int Id { get; set; }
        public CultureUpdateDto CultureUpdateDto { get; set; } = new();
    }

    public class DeleteCultureCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddCultureNationCommand : IRequest<bool>
    {
        public int CultureId { get; set; }
        public int NationId { get; set; }
    }

    public class RemoveCultureNationCommand : IRequest<bool>
    {
        public int CultureId { get; set; }
        public int NationId { get; set; }
    }

    public class AddCultureSapientSpeciesCommand : IRequest<bool>
    {
        public int CultureId { get; set; }
        public int SapientSpeciesId { get; set; }
    }

    public class RemoveCultureSapientSpeciesCommand : IRequest<bool>
    {
        public int CultureId { get; set; }
        public int SapientSpeciesId { get; set; }
    }

    public class AddCultureSocialClassCommand : IRequest<bool>
    {
        public int CultureId { get; set; }
        public int SocialClassId { get; set; }
    }

    public class RemoveCultureSocialClassCommand : IRequest<bool>
    {
        public int CultureId { get; set; }
        public int SocialClassId { get; set; }
    }
}
