using ChronicleKeeper.Core.DTOs.Hobby;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Hobbies.Queries
{
    public class GetAllHobbiesQuery : IRequest<List<HobbyDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetHobbyByIdQuery : IRequest<HobbyDetailsDto?>
    {
        public int Id { get; set; }
    }
}
