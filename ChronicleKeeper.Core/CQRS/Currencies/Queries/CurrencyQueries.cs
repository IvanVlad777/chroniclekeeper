using ChronicleKeeper.Core.DTOs.Currency;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Currencies.Queries
{
    public class GetAllCurrenciesQuery : IRequest<List<CurrencyDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetCurrencyByIdQuery : IRequest<CurrencyDetailsDto?>
    {
        public int Id { get; set; }
    }
}
