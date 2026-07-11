using ChronicleKeeper.Core.DTOs.WeatherPattern;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.WeatherPatterns.Commands
{
    public class CreateWeatherPatternCommand : IRequest<WeatherPatternDto>
    {
        public WeatherPatternCreateDto WeatherPatternCreateDto { get; set; } = new();
    }

    public class UpdateWeatherPatternCommand : IRequest<WeatherPatternDto>
    {
        public int Id { get; set; }
        public WeatherPatternUpdateDto WeatherPatternUpdateDto { get; set; } = new();
    }

    public class DeleteWeatherPatternCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
