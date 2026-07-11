using ChronicleKeeper.Core.DTOs.WeatherPattern;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.WeatherPatterns.Queries
{
    public class GetAllWeatherPatternsQuery : IRequest<List<WeatherPatternDto>>
    {
        public int? WorldId { get; set; }
        public int? ClimateZoneId { get; set; }
    }

    public class GetWeatherPatternByIdQuery : IRequest<WeatherPatternDto?>
    {
        public int Id { get; set; }
    }
}
