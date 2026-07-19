using ChronicleKeeper.Core.Entities.HistoryTimelines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class TimelineConfiguration : LoreEntityConfiguration<Timeline>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Timeline> builder)
        {
            // Pointer-only reference (poput Faction.LeaderId) — SetNull, ne blokira brisanje History-ja
            builder.HasOne(t => t.History)
                .WithMany(h => h.Timelines)
                .HasForeignKey(t => t.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(t => t.HistoryId);
        }
    }

    public class TimelineEventConfiguration : LoreEntityConfiguration<TimelineEvent>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TimelineEvent> builder)
        {
            builder.Property(e => e.Date)
                .HasMaxLength(100);

            builder.Property(e => e.Era)
                .HasMaxLength(100);

            builder.Property(e => e.Consequences)
                .HasMaxLength(2000);

            builder.HasOne(e => e.Timeline)
                .WithMany(t => t.Events)
                .HasForeignKey(e => e.TimelineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Pointer-only "where" reference — SetNull, doesn't block deleting the location.
            builder.HasOne(e => e.Location)
                .WithMany(l => l.TimelineEvents)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optional pointers to a Battle / Folklore — SetNull, don't block deleting those entities.
            builder.HasOne(e => e.Battle)
                .WithMany()
                .HasForeignKey(e => e.BattleId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.Folklore)
                .WithMany()
                .HasForeignKey(e => e.FolkloreId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => new { e.TimelineId, e.SortOrder });
            builder.HasIndex(e => e.LocationId);
            builder.HasIndex(e => e.BattleId);
            builder.HasIndex(e => e.FolkloreId);
        }
    }

    public class TimelineEventCharacterConfiguration : IEntityTypeConfiguration<TimelineEventCharacter>
    {
        public void Configure(EntityTypeBuilder<TimelineEventCharacter> builder)
        {
            builder.HasKey(x => new { x.TimelineEventId, x.CharacterId });

            builder.HasOne(x => x.TimelineEvent)
                .WithMany(e => e.InvolvedCharacters)
                .HasForeignKey(x => x.TimelineEventId)
                .OnDelete(DeleteBehavior.Cascade);

            // The parents (TimelineEvent, Character) share no cascading ancestor
            // (WorldId is always Restrict), so both sides may cascade.
            builder.HasOne(x => x.Character)
                .WithMany()
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
