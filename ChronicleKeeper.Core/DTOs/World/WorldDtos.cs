using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.World
{
    public class WorldDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class WorldCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;
    }

    // Update trenutno ima ista polja i pravila kao Create — nasljeđivanjem
    // izbjegavamo dupliciranje validacije; razdvojiti kad se polja raziđu.
    public class WorldUpdateDto : WorldCreateDto { }
}
