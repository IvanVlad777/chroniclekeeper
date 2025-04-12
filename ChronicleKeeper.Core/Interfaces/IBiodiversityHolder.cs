using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;

namespace ChronicleKeeper.Core.Interfaces
{
    public interface IBiodiversityHolder
    {
        ICollection<Animal> Animals { get; set; }
        ICollection<Plant> Plants { get; set; }
        ICollection<Fungus> Fungi { get; set; }
    }
}
