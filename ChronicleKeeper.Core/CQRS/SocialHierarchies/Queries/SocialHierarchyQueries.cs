using ChronicleKeeper.Core.DTOs.SocialHierarchy;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.SocialHierarchies.Queries
{
    public class GetAllSocialHierarchiesQuery : IRequest<List<SocialHierarchyDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetSocialHierarchyByIdQuery : IRequest<SocialHierarchyDetailsDto?>
    {
        public int Id { get; set; }
    }
}
