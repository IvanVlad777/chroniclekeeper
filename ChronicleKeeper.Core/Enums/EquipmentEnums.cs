namespace ChronicleKeeper.Core.Enums
{
    public class EquipmentEnums
    {
        public enum AbilityType
        {
            Physical = 1,
            Magical = 2,
            Mental = 3,
            Technical = 4
        }

        public enum AbilityRank
        {
            Beginner = 1,
            Expert = 2,
            Master = 3
        }

        public enum ItemCategory
        {
            Weapon = 1,
            Armor = 2,
            Tool = 3,
            MagicalItem = 4
        }

        public enum ItemRarity
        {
            Common = 1,
            Uncommon = 2,
            Rare = 3,
            Legendary = 4,
            Mythical = 5
        }

        public enum OwnershipTransferReason
        {
            Stolen = 1,
            Inherited = 2,
            Gifted = 3,
            Lost = 4,
            Found = 5
        }
    }
}
