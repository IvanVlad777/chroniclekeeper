using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class CulturalInstitution : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        public string InstitutionType { get; set; } = string.Empty; // Theater, Museum, Art Gallery
        public bool IsGovernmentFunded { get; set; } // If state-controlled

        public ICollection<Character> NotableArtists { get; set; } = new List<Character>(); // People associated

        //[ForeignKey("Culture")]
        public int? CountryId { get; set; }
        public Culture? Culture { get; set; }

        //[ForeignKey("City")]
        public int? CityId { get; set; }
        public City? City { get; set; }
    }
}
