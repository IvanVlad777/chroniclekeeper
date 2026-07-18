using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using static ChronicleKeeper.Core.Enums.GlobalEnums;

namespace ChronicleKeeper.Core.DTOs.Mutation
{
    public class MutationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public MutationOrigin Origin { get; set; }
        public MutationEffect Effect { get; set; }
        public int? MutantCreatureId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MutationDetailsDto : MutationDto
    {
        public ReferenceDto? MutantCreature { get; set; }
        public ReferenceDto? History { get; set; }
    }

    public class MutationCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public MutationOrigin Origin { get; set; }
        public MutationEffect Effect { get; set; }
        public int? MutantCreatureId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class MutationUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public MutationOrigin Origin { get; set; }
        public MutationEffect Effect { get; set; }
        public int? MutantCreatureId { get; set; }
        public int? HistoryId { get; set; }
    }
}
