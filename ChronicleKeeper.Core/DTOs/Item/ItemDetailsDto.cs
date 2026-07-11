using ChronicleKeeper.Core.DTOs.OwnershipHistory;

namespace ChronicleKeeper.Core.DTOs.Item
{
    public class ItemDetailsDto : ItemDto
    {
        public ReferenceDto? CurrentOwner { get; set; }
        public ReferenceDto? StoredAt { get; set; }
        public ReferenceDto? Faction { get; set; }
        public List<OwnershipHistoryDto> OwnershipHistory { get; set; } = new();
    }
}
