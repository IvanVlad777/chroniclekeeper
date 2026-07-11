using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Apprenticeship
{
    public class ApprenticeshipDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int ProfessionId { get; set; }
        public int? TradeSchoolId { get; set; }
        public string? TradeSchoolName { get; set; }
        public int DurationYears { get; set; }
        public bool IsPaid { get; set; }
        public string SkillsTaught { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ApprenticeshipCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet naukovanja se izvodi iz zanimanja — ne šalje se WorldId.</summary>
        [Required]
        public int ProfessionId { get; set; }

        public int? TradeSchoolId { get; set; }

        public int DurationYears { get; set; }
        public bool IsPaid { get; set; }

        [StringLength(1000)]
        public string SkillsTaught { get; set; } = string.Empty;
    }

    public class ApprenticeshipUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public int? TradeSchoolId { get; set; }

        public int DurationYears { get; set; }
        public bool IsPaid { get; set; }

        [StringLength(1000)]
        public string SkillsTaught { get; set; } = string.Empty;
    }
}
