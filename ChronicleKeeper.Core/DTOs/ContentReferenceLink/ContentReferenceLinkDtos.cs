using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.ContentReferenceLink
{
    /// <summary>
    /// DTOs for the <c>Reference</c> entity (Core\Entities\Content\Reference.cs) — named
    /// "ContentReferenceLink" here to avoid clashing with the generic {Id,Name} ReferenceDto used
    /// everywhere else in the API. Links one narrative unit (Content/Chapter/Episode) to one
    /// world entity (Character/Location/Faction/Nation); all 7 FKs are independently optional,
    /// no "exactly one per side" enforcement (matches EducationRecord's SchoolId/UniversityId looseness).
    /// Not a LoreEntity — no Name/Description/WorldId.
    /// </summary>
    public class ContentReferenceLinkDto
    {
        public int Id { get; set; }
        public string Note { get; set; } = string.Empty;

        public int? ContentId { get; set; }
        public int? ChapterId { get; set; }
        public int? EpisodeId { get; set; }

        public int? CharacterId { get; set; }
        public int? LocationId { get; set; }
        public int? FactionId { get; set; }
        public int? NationId { get; set; }
    }

    public class ContentReferenceLinkCreateDto
    {
        [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters")]
        public string Note { get; set; } = string.Empty;

        public int? ContentId { get; set; }
        public int? ChapterId { get; set; }
        public int? EpisodeId { get; set; }

        public int? CharacterId { get; set; }
        public int? LocationId { get; set; }
        public int? FactionId { get; set; }
        public int? NationId { get; set; }
    }

    public class ContentReferenceLinkUpdateDto
    {
        [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters")]
        public string Note { get; set; } = string.Empty;

        public int? ContentId { get; set; }
        public int? ChapterId { get; set; }
        public int? EpisodeId { get; set; }

        public int? CharacterId { get; set; }
        public int? LocationId { get; set; }
        public int? FactionId { get; set; }
        public int? NationId { get; set; }
    }
}
