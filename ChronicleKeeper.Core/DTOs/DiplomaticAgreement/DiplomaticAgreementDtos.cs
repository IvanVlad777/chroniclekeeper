using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.DiplomaticAgreement
{
    public class DiplomaticAgreementDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string AgreementType { get; set; } = string.Empty;
        public string SignedDate { get; set; } = string.Empty;
        public string? TerminationDate { get; set; }
        public int? DurationYears { get; set; }
        public string Terms { get; set; } = string.Empty;
        public bool IsUnequal { get; set; }
        public int FirstNationId { get; set; }
        public int SecondNationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class DiplomaticAgreementDetailsDto : DiplomaticAgreementDto
    {
        public ReferenceDto FirstNation { get; set; } = new();
        public ReferenceDto SecondNation { get; set; } = new();
    }

    public class DiplomaticAgreementCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100, ErrorMessage = "Agreement type cannot exceed 100 characters")]
        public string AgreementType { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Signed date cannot exceed 100 characters")]
        public string SignedDate { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Termination date cannot exceed 100 characters")]
        public string? TerminationDate { get; set; }

        public int? DurationYears { get; set; }

        [StringLength(4000, ErrorMessage = "Terms cannot exceed 4000 characters")]
        public string Terms { get; set; } = string.Empty;

        public bool IsUnequal { get; set; }

        [Required]
        public int FirstNationId { get; set; }

        [Required]
        public int SecondNationId { get; set; }
    }

    public class DiplomaticAgreementUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Agreement type cannot exceed 100 characters")]
        public string AgreementType { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Signed date cannot exceed 100 characters")]
        public string SignedDate { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Termination date cannot exceed 100 characters")]
        public string? TerminationDate { get; set; }

        public int? DurationYears { get; set; }

        [StringLength(4000, ErrorMessage = "Terms cannot exceed 4000 characters")]
        public string Terms { get; set; } = string.Empty;

        public bool IsUnequal { get; set; }

        [Required]
        public int FirstNationId { get; set; }

        [Required]
        public int SecondNationId { get; set; }
    }
}
