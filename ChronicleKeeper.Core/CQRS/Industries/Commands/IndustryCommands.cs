using ChronicleKeeper.Core.DTOs.Industry;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Industries.Commands
{
    public class CreateIndustryCommand : IRequest<IndustryDto>
    {
        public IndustryCreateDto IndustryCreateDto { get; set; } = new();
    }

    public class UpdateIndustryCommand : IRequest<IndustryDto>
    {
        public int Id { get; set; }
        public IndustryUpdateDto IndustryUpdateDto { get; set; } = new();
    }

    public class DeleteIndustryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
