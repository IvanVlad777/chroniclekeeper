using ChronicleKeeper.Core.DTOs.PrivilegeLaw;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.PrivilegeLaws.Queries
{
    public class GetAllPrivilegeLawsQuery : IRequest<List<PrivilegeLawDto>>
    {
        public int? WorldId { get; set; }
        public int? SocialClassId { get; set; }
    }

    public class GetPrivilegeLawByIdQuery : IRequest<PrivilegeLawDto?>
    {
        public int Id { get; set; }
    }
}
