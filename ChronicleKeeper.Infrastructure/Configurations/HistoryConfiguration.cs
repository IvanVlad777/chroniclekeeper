using ChronicleKeeper.Core.Entities.HistoryTimelines;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class HistoryConfiguration : LoreEntityConfiguration<History>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<History> builder)
        {
            builder.Property(h => h.Summary).HasMaxLength(4000);
        }
    }
}
