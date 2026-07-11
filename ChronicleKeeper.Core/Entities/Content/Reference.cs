using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Entities.Content.Movie;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Content
{
    /// <summary>
    /// Links one narrative unit (Content/Chapter/Episode — exactly one expected, not enforced)
    /// to one world entity (Character/Location/Faction/Nation — exactly one expected, not enforced),
    /// recording where that entity appears. Not a LoreEntity — pure join-style record.
    /// </summary>
    public class Reference
    {
        [Key]
        public int Id { get; set; }

        public string Note { get; set; } = string.Empty;

        // Narrative-unit side (pick one)
        public int? ContentId { get; set; }
        public virtual Content? Content { get; set; }

        public int? ChapterId { get; set; }
        public virtual Chapter? Chapter { get; set; }

        public int? EpisodeId { get; set; }
        public virtual Episode? Episode { get; set; }

        // World-entity side (pick one)
        public int? CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int? LocationId { get; set; }
        public virtual Location? Location { get; set; }

        public int? FactionId { get; set; }
        public virtual Faction? Faction { get; set; }

        public int? NationId { get; set; }
        public virtual Nation? Nation { get; set; }
    }
}
