using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Interfaces
{
    public interface ILoreEntity
    {
        [Key]
        int Id { get; set; }
        [Required]
        string Name { get; set; }
        string Description { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        // History? History { get; set; }
    }
}
