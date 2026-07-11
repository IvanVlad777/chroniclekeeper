using AutoMapper;
using ChronicleKeeper.Core.DTOs.Episode;
using ChronicleKeeper.Core.Entities.Content.Movie;

public class EpisodeProfile : Profile
{
    public EpisodeProfile()
    {
        CreateMap<Episode, EpisodeDto>();
        CreateMap<EpisodeCreateDto, Episode>();
        CreateMap<EpisodeUpdateDto, Episode>();
    }
}
