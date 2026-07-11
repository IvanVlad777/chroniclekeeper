using ChronicleKeeper.Core.DTOs.Item;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Items.Commands
{
    public class CreateItemCommand : IRequest<ItemDto>
    {
        public ItemCreateDto ItemCreateDto { get; set; } = new();
    }

    public class UpdateItemCommand : IRequest<ItemDto>
    {
        public int Id { get; set; }
        public ItemUpdateDto ItemUpdateDto { get; set; } = new();
    }

    public class DeleteItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
