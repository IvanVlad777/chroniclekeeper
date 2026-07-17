using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Military
{
    // ---------------- MilitaryDoctrine ----------------
    public class MilitaryDoctrineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Strategy { get; set; } = string.Empty;
        public string Philosophy { get; set; } = string.Empty;
        public bool PrioritizesInfantry { get; set; }
        public bool PrioritizesCavalry { get; set; }
        public bool PrioritizesArtillery { get; set; }
        public bool PrioritizesNavalForces { get; set; }
        public bool PrioritizesAirForces { get; set; }
        public bool RequiresHeavyIndustry { get; set; }
        public bool UsesMercenaries { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MilitaryDoctrineDetailsDto : MilitaryDoctrineDto
    {
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Organizations { get; set; } = new();
    }

    public class MilitaryDoctrineCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(200)] public string Strategy { get; set; } = string.Empty;
        [StringLength(200)] public string Philosophy { get; set; } = string.Empty;
        public bool PrioritizesInfantry { get; set; }
        public bool PrioritizesCavalry { get; set; }
        public bool PrioritizesArtillery { get; set; }
        public bool PrioritizesNavalForces { get; set; }
        public bool PrioritizesAirForces { get; set; }
        public bool RequiresHeavyIndustry { get; set; }
        public bool UsesMercenaries { get; set; }
    }

    public class MilitaryDoctrineUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(200)] public string Strategy { get; set; } = string.Empty;
        [StringLength(200)] public string Philosophy { get; set; } = string.Empty;
        public bool PrioritizesInfantry { get; set; }
        public bool PrioritizesCavalry { get; set; }
        public bool PrioritizesArtillery { get; set; }
        public bool PrioritizesNavalForces { get; set; }
        public bool PrioritizesAirForces { get; set; }
        public bool RequiresHeavyIndustry { get; set; }
        public bool UsesMercenaries { get; set; }
    }

    // ---------------- MilitaryOrganization ----------------
    public class MilitaryOrganizationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? MilitaryDoctrineId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MilitaryOrganizationDetailsDto : MilitaryOrganizationDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? MilitaryDoctrine { get; set; }
        public List<ReferenceDto> Armies { get; set; } = new();
        public List<ReferenceDto> Countries { get; set; } = new();
        public List<ReferenceDto> Factions { get; set; } = new();
    }

    public class MilitaryOrganizationCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string Type { get; set; } = string.Empty;
        [StringLength(100)] public string Role { get; set; } = string.Empty;
        public int? MilitaryDoctrineId { get; set; }
    }

    public class MilitaryOrganizationUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string Type { get; set; } = string.Empty;
        [StringLength(100)] public string Role { get; set; } = string.Empty;
        public int? MilitaryDoctrineId { get; set; }
    }

    // ---------------- Army ----------------
    public class ArmyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public bool IsStandingArmy { get; set; }
        public int Size { get; set; }
        public int? CityId { get; set; }
        public int? MilitaryOrganizationId { get; set; }
        public int? FactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ArmyDetailsDto : ArmyDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? City { get; set; }
        public ReferenceDto? MilitaryOrganization { get; set; }
        public ReferenceDto? Faction { get; set; }
        public List<ReferenceDto> Units { get; set; } = new();
        public List<ReferenceDto> Battles { get; set; } = new();
    }

    public class ArmyCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public bool IsStandingArmy { get; set; }
        [Range(0, int.MaxValue)] public int Size { get; set; }
        public int? CityId { get; set; }
        public int? MilitaryOrganizationId { get; set; }
        public int? FactionId { get; set; }
    }

    public class ArmyUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        public bool IsStandingArmy { get; set; }
        [Range(0, int.MaxValue)] public int Size { get; set; }
        public int? CityId { get; set; }
        public int? MilitaryOrganizationId { get; set; }
        public int? FactionId { get; set; }
    }

    // ---------------- MilitaryUnit (child of Army) ----------------
    public class MilitaryUnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string UnitType { get; set; } = string.Empty;
        public int Size { get; set; }
        public bool IsElite { get; set; }
        public int BelongsToArmyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MilitaryUnitDetailsDto : MilitaryUnitDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? BelongsToArmy { get; set; }
        public List<ReferenceDto> Ranks { get; set; } = new();
        public List<ReferenceDto> Equipment { get; set; } = new();
    }

    public class MilitaryUnitCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string UnitType { get; set; } = string.Empty;
        [Range(0, int.MaxValue)] public int Size { get; set; }
        public bool IsElite { get; set; }
        [Required]
        public int BelongsToArmyId { get; set; }
    }

    public class MilitaryUnitUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string UnitType { get; set; } = string.Empty;
        [Range(0, int.MaxValue)] public int Size { get; set; }
        public bool IsElite { get; set; }
    }

    // ---------------- MilitaryRank (child of MilitaryUnit) ----------------
    public class MilitaryRankDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string RankTitle { get; set; } = string.Empty;
        public int RankLevel { get; set; }
        public int MilitaryUnitId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MilitaryRankCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string RankTitle { get; set; } = string.Empty;
        public int RankLevel { get; set; }
        [Required]
        public int MilitaryUnitId { get; set; }
    }

    public class MilitaryRankUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string RankTitle { get; set; } = string.Empty;
        public int RankLevel { get; set; }
    }

    // ---------------- Battle ----------------
    public class BattleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string? BattleDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Outcome { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BattleDetailsDto : BattleDto
    {
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> ParticipatingArmies { get; set; } = new();
    }

    public class BattleCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string? BattleDate { get; set; }
        [StringLength(200)] public string Location { get; set; } = string.Empty;
        [StringLength(200)] public string Outcome { get; set; } = string.Empty;
    }

    public class BattleUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string? BattleDate { get; set; }
        [StringLength(200)] public string Location { get; set; } = string.Empty;
        [StringLength(200)] public string Outcome { get; set; } = string.Empty;
    }

    // ---------------- MilitaryEquipment ----------------
    public class MilitaryEquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string EquipmentType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MilitaryEquipmentDetailsDto : MilitaryEquipmentDto
    {
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Units { get; set; } = new();
    }

    public class MilitaryEquipmentCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string EquipmentType { get; set; } = string.Empty;
    }

    public class MilitaryEquipmentUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string EquipmentType { get; set; } = string.Empty;
    }
}
