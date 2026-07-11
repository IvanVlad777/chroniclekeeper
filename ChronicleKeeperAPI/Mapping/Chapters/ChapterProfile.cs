using AutoMapper;
using ChronicleKeeper.Core.DTOs.Chapter;
using ChronicleKeeper.Core.Entities.Content.Book;

public class ChapterProfile : Profile
{
    public ChapterProfile()
    {
        CreateMap<Chapter, ChapterDto>();
        CreateMap<ChapterCreateDto, Chapter>();
        CreateMap<ChapterUpdateDto, Chapter>();
    }
}
