using ChronicleKeeper.Core.DTOs.Currency;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Currencies.Commands
{
    public class CreateCurrencyCommand : IRequest<CurrencyDto>
    {
        public CurrencyCreateDto CurrencyCreateDto { get; set; } = new();
    }

    public class UpdateCurrencyCommand : IRequest<CurrencyDto>
    {
        public int Id { get; set; }
        public CurrencyUpdateDto CurrencyUpdateDto { get; set; } = new();
    }

    public class DeleteCurrencyCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
