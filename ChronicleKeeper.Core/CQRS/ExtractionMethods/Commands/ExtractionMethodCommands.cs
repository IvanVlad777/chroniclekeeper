using ChronicleKeeper.Core.DTOs.ExtractionMethod;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ExtractionMethods.Commands
{
    public class CreateExtractionMethodCommand : IRequest<ExtractionMethodDto>
    {
        public ExtractionMethodCreateDto ExtractionMethodCreateDto { get; set; } = new();
    }

    public class UpdateExtractionMethodCommand : IRequest<ExtractionMethodDto>
    {
        public int Id { get; set; }
        public ExtractionMethodUpdateDto ExtractionMethodUpdateDto { get; set; } = new();
    }

    public class DeleteExtractionMethodCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
