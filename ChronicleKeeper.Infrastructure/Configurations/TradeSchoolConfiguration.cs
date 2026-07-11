using ChronicleKeeper.Core.Entities.Professions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // TPH leaf — TradeSchool shares the "Schools" table (via SchoolConfiguration's
    // HasDiscriminator), so this config only touches TradeSchool's own extra columns.
    public class TradeSchoolConfiguration : IEntityTypeConfiguration<TradeSchool>
    {
        public void Configure(EntityTypeBuilder<TradeSchool> builder)
        {
            builder.Property(t => t.Specialization)
                .HasMaxLength(200);
        }
    }
}
