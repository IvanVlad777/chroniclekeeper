using ChronicleKeeper.Core.DTOs.BankingSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.BankingSystems.Commands
{
    public class CreateBankingSystemCommand : IRequest<BankingSystemDto>
    {
        public BankingSystemCreateDto BankingSystemCreateDto { get; set; } = new();
    }

    public class UpdateBankingSystemCommand : IRequest<BankingSystemDto>
    {
        public int Id { get; set; }
        public BankingSystemUpdateDto BankingSystemUpdateDto { get; set; } = new();
    }

    public class DeleteBankingSystemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
