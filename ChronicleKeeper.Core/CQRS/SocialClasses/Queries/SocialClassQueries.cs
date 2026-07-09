using ChronicleKeeper.Core.DTOs.SocialClass;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.SocialClasses.Queries
{
    public class GetAllSocialClassesQuery : IRequest<List<SocialClassDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetSocialClassByIdQuery : IRequest<SocialClassDetailsDto?>
    {
        public int Id { get; set; }
    }
}
