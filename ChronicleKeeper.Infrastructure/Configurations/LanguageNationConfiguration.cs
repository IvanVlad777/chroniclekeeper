using ChronicleKeeper.Core.Entities.Social.Cultures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class LanguageNationConfiguration : IEntityTypeConfiguration<LanguageNation>
    {
        public void Configure(EntityTypeBuilder<LanguageNation> builder)
        {
            builder.HasKey(ln => new { ln.LanguageId, ln.NationId });

            builder.HasOne(ln => ln.Language)
                .WithMany(l => l.Nations)
                .HasForeignKey(ln => ln.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ln => ln.Nation)
                .WithMany()
                .HasForeignKey(ln => ln.NationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ln => ln.NationId);
        }
    }
}
