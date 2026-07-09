using ChronicleKeeper.Core.DTOs.Language;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Languages.Commands
{
    public class CreateLanguageCommand : IRequest<LanguageDto>
    {
        public LanguageCreateDto LanguageCreateDto { get; set; } = new();
    }

    public class UpdateLanguageCommand : IRequest<LanguageDto>
    {
        public int Id { get; set; }
        public LanguageUpdateDto LanguageUpdateDto { get; set; } = new();
    }

    public class DeleteLanguageCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
