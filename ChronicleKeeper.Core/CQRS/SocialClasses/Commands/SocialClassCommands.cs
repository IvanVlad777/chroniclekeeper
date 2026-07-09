using ChronicleKeeper.Core.DTOs.SocialClass;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.SocialClasses.Commands
{
    public class CreateSocialClassCommand : IRequest<SocialClassDto>
    {
        public SocialClassCreateDto SocialClassCreateDto { get; set; } = new();
    }

    public class UpdateSocialClassCommand : IRequest<SocialClassDto>
    {
        public int Id { get; set; }
        public SocialClassUpdateDto SocialClassUpdateDto { get; set; } = new();
    }

    public class DeleteSocialClassCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
