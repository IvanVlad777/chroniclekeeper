using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.UniversityMajor
{
    public class UniversityMajorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int UniversityId { get; set; }
        public string MajorName { get; set; } = string.Empty;
        public string DegreeLevel { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UniversityMajorDetailsDto : UniversityMajorDto
    {
        public ReferenceDto? University { get; set; }
        public List<ReferenceDto> Professors { get; set; } = new();
        public List<ReferenceDto> Students { get; set; } = new();
    }

    public class UniversityMajorCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet smjera se izvodi iz sveučilišta — ne šalje se WorldId.</summary>
        [Required]
        public int UniversityId { get; set; }

        [StringLength(100)]
        public string MajorName { get; set; } = string.Empty;

        [StringLength(100)]
        public string DegreeLevel { get; set; } = string.Empty;
    }

    public class UniversityMajorUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string MajorName { get; set; } = string.Empty;

        [StringLength(100)]
        public string DegreeLevel { get; set; } = string.Empty;
    }
}
