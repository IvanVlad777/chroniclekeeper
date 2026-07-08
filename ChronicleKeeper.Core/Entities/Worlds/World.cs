using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Worlds
{
    /// <summary>
    /// Aggregate root: a writer's world/project. Every lore entity belongs to exactly one World.
    /// Deliberately has no collection navigations — deleting a world goes through
    /// WorldRepository.DeleteAsync (ordered row deletes), and JSON serialization stays cycle-free.
    /// </summary>
    public class World
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>FK to AspNetUsers.Id (Identity user who owns this world).</summary>
        [Required]
        public string OwnerId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
