using ChronicleKeeper.Core.DTOs.SocialHierarchy;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.SocialHierarchies.Commands
{
    public class CreateSocialHierarchyCommand : IRequest<SocialHierarchyDto>
    {
        public SocialHierarchyCreateDto SocialHierarchyCreateDto { get; set; } = new();
    }

    public class UpdateSocialHierarchyCommand : IRequest<SocialHierarchyDto>
    {
        public int Id { get; set; }
        public SocialHierarchyUpdateDto SocialHierarchyUpdateDto { get; set; } = new();
    }

    public class DeleteSocialHierarchyCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
