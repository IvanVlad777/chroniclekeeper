using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.CultureDetails
{
    // ==================== Custom ====================
    public class CustomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public bool IsUniversal { get; set; }
        public int? CultureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CustomDetailsDto : CustomDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
    }

    public class CustomCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public bool IsUniversal { get; set; }
        public int? CultureId { get; set; }
    }

    public class CustomUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        public bool IsUniversal { get; set; }
        public int? CultureId { get; set; }
    }

    // ==================== ArtForm ====================
    public class ArtFormDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string NotableArtists { get; set; } = string.Empty;
        public string HistoricalInfluences { get; set; } = string.Empty;
        public int CultureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ArtFormDetailsDto : ArtFormDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
    }

    public class ArtFormCreateDto
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
        [StringLength(500)] public string NotableArtists { get; set; } = string.Empty;
        [StringLength(500)] public string HistoricalInfluences { get; set; } = string.Empty;
        [Required]
        public int CultureId { get; set; }
    }

    public class ArtFormUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string Type { get; set; } = string.Empty;
        [StringLength(500)] public string NotableArtists { get; set; } = string.Empty;
        [StringLength(500)] public string HistoricalInfluences { get; set; } = string.Empty;
        [Required]
        public int CultureId { get; set; }
    }

    // ==================== Cuisine ====================
    public class CuisineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string MainIngredients { get; set; } = string.Empty;
        public string CookingMethods { get; set; } = string.Empty;
        public bool IsVegetarian { get; set; }
        public string TypicalDishes { get; set; } = string.Empty;
        public int CultureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CuisineDetailsDto : CuisineDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
    }

    public class CuisineCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(500)] public string MainIngredients { get; set; } = string.Empty;
        [StringLength(500)] public string CookingMethods { get; set; } = string.Empty;
        public bool IsVegetarian { get; set; }
        [StringLength(500)] public string TypicalDishes { get; set; } = string.Empty;
        [Required]
        public int CultureId { get; set; }
    }

    public class CuisineUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(500)] public string MainIngredients { get; set; } = string.Empty;
        [StringLength(500)] public string CookingMethods { get; set; } = string.Empty;
        public bool IsVegetarian { get; set; }
        [StringLength(500)] public string TypicalDishes { get; set; } = string.Empty;
        [Required]
        public int CultureId { get; set; }
    }

    // ==================== Clothing ====================
    public class ClothingDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string ClothingType { get; set; } = string.Empty;
        public string Materials { get; set; } = string.Empty;
        public string DesignFeatures { get; set; } = string.Empty;
        public bool IsRitualistic { get; set; }
        public bool IsArmor { get; set; }
        public string SpecialProperties { get; set; } = string.Empty;
        public int CultureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ClothingDetailsDto : ClothingDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
    }

    public class ClothingCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string ClothingType { get; set; } = string.Empty;
        [StringLength(500)] public string Materials { get; set; } = string.Empty;
        [StringLength(500)] public string DesignFeatures { get; set; } = string.Empty;
        public bool IsRitualistic { get; set; }
        public bool IsArmor { get; set; }
        [StringLength(500)] public string SpecialProperties { get; set; } = string.Empty;
        [Required]
        public int CultureId { get; set; }
    }

    public class ClothingUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string ClothingType { get; set; } = string.Empty;
        [StringLength(500)] public string Materials { get; set; } = string.Empty;
        [StringLength(500)] public string DesignFeatures { get; set; } = string.Empty;
        public bool IsRitualistic { get; set; }
        public bool IsArmor { get; set; }
        [StringLength(500)] public string SpecialProperties { get; set; } = string.Empty;
        [Required]
        public int CultureId { get; set; }
    }

    // ==================== Tradition ====================
    public class TraditionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Practice { get; set; } = string.Empty;
        public bool IsReligious { get; set; }
        public int? ReligionId { get; set; }
        public int CultureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TraditionDetailsDto : TraditionDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
        public ReferenceDto? Religion { get; set; }
    }

    public class TraditionCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(500)] public string Practice { get; set; } = string.Empty;
        public bool IsReligious { get; set; }
        public int? ReligionId { get; set; }
        [Required]
        public int CultureId { get; set; }
    }

    public class TraditionUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(500)] public string Practice { get; set; } = string.Empty;
        public bool IsReligious { get; set; }
        public int? ReligionId { get; set; }
        [Required]
        public int CultureId { get; set; }
    }
}
