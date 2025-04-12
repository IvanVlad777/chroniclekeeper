using ChronicleKeeper.Core.Entities.Miscellaneous;

namespace ChronicleKeeper.Core.Interfaces
{
    public interface IMutant
    {
        ICollection<Mutation> Mutations { get; set; }
    }
}
