using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.DTOs.Mythology
{
    // ---------------- ReligiousOrder ----------------
    public class ReligiousOrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Beliefs { get; set; } = string.Empty;
        public bool IsMilitant { get; set; }
        public bool IsSecretive { get; set; }
        public int? ReligionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReligiousOrderDetailsDto : ReligiousOrderDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Religion { get; set; }
        // Read-only reverse list of clergy training programs (ReligiousEducation.ReligiousOrderId).
        public List<ReferenceDto> ClergyTraining { get; set; } = new();
        // Read-only reverse list — the write side lives on Deity.
        public List<ReferenceDto> Deities { get; set; } = new();
        // Owner-side M:N — add/remove endpoints live here.
        public List<ReferenceDto> Factions { get; set; } = new();
    }

    public class ReligiousOrderCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string OrderType { get; set; } = string.Empty;
        [StringLength(2000)] public string Beliefs { get; set; } = string.Empty;
        public bool IsMilitant { get; set; }
        public bool IsSecretive { get; set; }
        public int? ReligionId { get; set; }
    }

    public class ReligiousOrderUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string OrderType { get; set; } = string.Empty;
        [StringLength(2000)] public string Beliefs { get; set; } = string.Empty;
        public bool IsMilitant { get; set; }
        public bool IsSecretive { get; set; }
        public int? ReligionId { get; set; }
    }

    // ---------------- Deity ----------------
    public class DeityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Domain { get; set; } = string.Empty;
        public string WorshipMethods { get; set; } = string.Empty;
        public bool IsMonotheistic { get; set; }
        public DeityType DeityType { get; set; }
        public bool IsImmortal { get; set; }
        public bool CanManifestPhysically { get; set; }
        public bool GrantsPowers { get; set; }
        public int? ReligionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class DeityDetailsDto : DeityDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Religion { get; set; }
        // Read-only 1:N reverse lists (FK lives on the child).
        public List<ReferenceDto> SacredTexts { get; set; } = new();
        public List<ReferenceDto> SacredSitesOfDeity { get; set; } = new();
        public List<ReferenceDto> MajorMyths { get; set; } = new();
        // Owner-side M:N — add/remove endpoints live here.
        public List<ReferenceDto> OrdersDedicatedToDeity { get; set; } = new();
        public List<ReferenceDto> AlliedDeities { get; set; } = new();
        public List<ReferenceDto> RivalDeities { get; set; } = new();
    }

    public class DeityCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(200)] public string Domain { get; set; } = string.Empty;
        [StringLength(2000)] public string WorshipMethods { get; set; } = string.Empty;
        public bool IsMonotheistic { get; set; }
        public DeityType DeityType { get; set; }
        public bool IsImmortal { get; set; }
        public bool CanManifestPhysically { get; set; }
        public bool GrantsPowers { get; set; }
        public int? ReligionId { get; set; }
    }

    public class DeityUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(200)] public string Domain { get; set; } = string.Empty;
        [StringLength(2000)] public string WorshipMethods { get; set; } = string.Empty;
        public bool IsMonotheistic { get; set; }
        public DeityType DeityType { get; set; }
        public bool IsImmortal { get; set; }
        public bool CanManifestPhysically { get; set; }
        public bool GrantsPowers { get; set; }
        public int? ReligionId { get; set; }
    }
}
