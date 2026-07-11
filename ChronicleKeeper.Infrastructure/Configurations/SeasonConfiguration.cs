using ChronicleKeeper.Core.Entities.Geography.Climate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class SeasonConfiguration : LoreEntityConfiguration<Season>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Season> builder)
        {
            builder.HasOne(s => s.History)
                .WithMany()
                .HasForeignKey(s => s.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
