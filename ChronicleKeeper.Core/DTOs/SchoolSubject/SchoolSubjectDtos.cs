using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.SchoolSubject
{
    public class SchoolSubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int SchoolId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public bool IsMandatory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SchoolSubjectCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet predmeta se izvodi iz škole — ne šalje se WorldId.</summary>
        [Required]
        public int SchoolId { get; set; }

        [StringLength(100)]
        public string SubjectName { get; set; } = string.Empty;

        public bool IsMandatory { get; set; }
    }

    public class SchoolSubjectUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string SubjectName { get; set; } = string.Empty;

        public bool IsMandatory { get; set; }
    }
}
