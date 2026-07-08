using AutoMapper;
using ChronicleKeeper.Core.DTOs.Note;
using NoteEntity = ChronicleKeeper.Core.Entities.Notes.Note;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteEntity, NoteDto>();
        CreateMap<NoteCreateDto, NoteEntity>();
        CreateMap<NoteUpdateDto, NoteEntity>();
    }
}
