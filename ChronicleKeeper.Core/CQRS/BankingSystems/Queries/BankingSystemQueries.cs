using ChronicleKeeper.Core.DTOs.BankingSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.BankingSystems.Queries
{
    public class GetAllBankingSystemsQuery : IRequest<List<BankingSystemDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetBankingSystemByIdQuery : IRequest<BankingSystemDetailsDto?>
    {
        public int Id { get; set; }
    }
}
