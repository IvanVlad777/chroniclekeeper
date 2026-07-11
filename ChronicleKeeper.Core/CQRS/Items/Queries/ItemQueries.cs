using ChronicleKeeper.Core.DTOs.Item;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Items.Queries
{
    public class GetAllItemsQuery : IRequest<List<ItemDto>>
    {
        public int? WorldId { get; set; }
        public int? CurrentOwnerId { get; set; }
        public int? FactionId { get; set; }
    }

    public class GetItemByIdQuery : IRequest<ItemDetailsDto?>
    {
        public int Id { get; set; }
    }
}
