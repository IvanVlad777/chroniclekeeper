using ChronicleKeeper.Core.Entities.HistoryTimelines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class TimelineConfiguration : LoreEntityConfiguration<Timeline>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Timeline> builder)
        {
        }
    }

    public class TimelineEventConfiguration : LoreEntityConfiguration<TimelineEvent>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TimelineEvent> builder)
        {
            builder.Property(e => e.Date)
                .HasMaxLength(100);

            builder.Property(e => e.Consequences)
                .HasMaxLength(2000);

            builder.HasOne(e => e.Timeline)
                .WithMany(t => t.Events)
                .HasForeignKey(e => e.TimelineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.TimelineId, e.SortOrder });
        }
    }
}
