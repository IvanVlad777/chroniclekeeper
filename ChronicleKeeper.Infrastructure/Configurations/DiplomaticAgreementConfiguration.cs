using ChronicleKeeper.Core.Entities.Social.Politics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class DiplomaticAgreementConfiguration : LoreEntityConfiguration<DiplomaticAgreement>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DiplomaticAgreement> builder)
        {
            builder.Property(a => a.AgreementType)
                .HasMaxLength(100);

            builder.Property(a => a.SignedDate)
                .HasMaxLength(100);

            builder.Property(a => a.TerminationDate)
                .HasMaxLength(100);

            builder.Property(a => a.Terms)
                .HasMaxLength(4000);

            // Two FKs to the same table (Nation) — both Restrict, no cascade needed;
            // a treaty must be resolved/deleted before either signatory nation can be.
            builder.HasOne(a => a.FirstNation)
                .WithMany()
                .HasForeignKey(a => a.FirstNationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.SecondNation)
                .WithMany()
                .HasForeignKey(a => a.SecondNationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
