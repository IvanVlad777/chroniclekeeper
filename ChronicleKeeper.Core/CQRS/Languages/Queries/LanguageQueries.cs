using ChronicleKeeper.Core.DTOs.Language;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Languages.Queries
{
    public class GetAllLanguagesQuery : IRequest<List<LanguageDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetLanguageByIdQuery : IRequest<LanguageDetailsDto?>
    {
        public int Id { get; set; }
    }
}
