using ChronicleKeeper.Core.DTOs.PrivilegeLaw;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.PrivilegeLaws.Commands
{
    public class CreatePrivilegeLawCommand : IRequest<PrivilegeLawDto>
    {
        public PrivilegeLawCreateDto PrivilegeLawCreateDto { get; set; } = new();
    }

    public class UpdatePrivilegeLawCommand : IRequest<PrivilegeLawDto>
    {
        public int Id { get; set; }
        public PrivilegeLawUpdateDto PrivilegeLawUpdateDto { get; set; } = new();
    }

    public class DeletePrivilegeLawCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
