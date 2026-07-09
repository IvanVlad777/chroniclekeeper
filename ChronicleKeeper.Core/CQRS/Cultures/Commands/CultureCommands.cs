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
}
