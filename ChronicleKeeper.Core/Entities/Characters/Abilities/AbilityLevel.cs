﻿using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Characters.Abilities
{
    public class AbilityLevel : ILoreEntity
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
        public string Rank { get; set; } = string.Empty; // Beginner, Expert, Master

        //[ForeignKey("AbilityId")]
        public int? AbilityId { get; set; }
        public Ability? Ability { get; set; }
    }
}
