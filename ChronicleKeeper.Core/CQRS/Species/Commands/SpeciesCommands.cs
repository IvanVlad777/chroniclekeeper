using ChronicleKeeper.Core.DTOs.Species;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Species.Commands
{
    public class CreateSpeciesCommand : IRequest<SpeciesDto>
    {
        public SpeciesCreateDto SpeciesCreateDto { get; set; } = new();
    }

    public class UpdateSpeciesCommand : IRequest<SpeciesDto>
    {
        public int Id { get; set; }
        public SpeciesUpdateDto SpeciesUpdateDto { get; set; } = new();
    }

    public class DeleteSpeciesCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class CreateRaceCommand : IRequest<RaceDto>
    {
        public RaceCreateDto RaceCreateDto { get; set; } = new();
    }

    public class UpdateRaceCommand : IRequest<RaceDto>
    {
        public int Id { get; set; }
        public RaceUpdateDto RaceUpdateDto { get; set; } = new();
    }

    public class DeleteRaceCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
