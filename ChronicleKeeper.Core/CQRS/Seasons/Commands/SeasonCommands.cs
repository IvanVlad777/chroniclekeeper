using ChronicleKeeper.Core.DTOs.Season;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Seasons.Commands
{
    public class CreateSeasonCommand : IRequest<SeasonDto>
    {
        public SeasonCreateDto SeasonCreateDto { get; set; } = new();
    }

    public class UpdateSeasonCommand : IRequest<SeasonDto>
    {
        public int Id { get; set; }
        public SeasonUpdateDto SeasonUpdateDto { get; set; } = new();
    }

    public class DeleteSeasonCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
