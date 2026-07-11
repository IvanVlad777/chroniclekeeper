using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.JobRank;
using ChronicleKeeper.Core.DTOs.Apprenticeship;
using ChronicleKeeper.Core.DTOs.Specialisation;

namespace ChronicleKeeper.Core.DTOs.Profession
{
    public class ProfessionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string RequiredSkills { get; set; } = string.Empty;
        public string WorkEnvironment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ProfessionDetailsDto : ProfessionDto
    {
        public List<JobRankDto> JobRanks { get; set; } = new();
        public List<ApprenticeshipDto> Apprenticeships { get; set; } = new();
        public List<SpecialisationDto> Specialisations { get; set; } = new();
        public List<ReferenceDto> PracticedBySpecies { get; set; } = new();
        public List<ReferenceDto> SocialClasses { get; set; } = new();
        public List<ReferenceDto> TradeSchools { get; set; } = new();
        public List<ReferenceDto> Characters { get; set; } = new();
    }

    public class ProfessionCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(500)]
        public string RequiredSkills { get; set; } = string.Empty;

        [StringLength(500)]
        public string WorkEnvironment { get; set; } = string.Empty;
    }

    public class ProfessionUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string RequiredSkills { get; set; } = string.Empty;

        [StringLength(500)]
        public string WorkEnvironment { get; set; } = string.Empty;
    }
}
