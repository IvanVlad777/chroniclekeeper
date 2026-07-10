using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.DTOs.LegalSystem
{
    public class LegalSystemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string Laws { get; set; } = string.Empty;
        public ScaleLevel JudicialIndependence { get; set; }
        public PunishmentMethods PunishmentMethods { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LegalSystemDetailsDto : LegalSystemDto
    {
    }

    public class LegalSystemCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(4000, ErrorMessage = "Laws cannot exceed 4000 characters")]
        public string Laws { get; set; } = string.Empty;

        public ScaleLevel JudicialIndependence { get; set; }
        public PunishmentMethods PunishmentMethods { get; set; }
    }

    public class LegalSystemUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Laws cannot exceed 4000 characters")]
        public string Laws { get; set; } = string.Empty;

        public ScaleLevel JudicialIndependence { get; set; }
        public PunishmentMethods PunishmentMethods { get; set; }
    }
}
