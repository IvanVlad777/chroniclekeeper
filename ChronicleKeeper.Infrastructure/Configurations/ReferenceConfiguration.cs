using ChronicleKeeper.Core.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Pure link record (poput CultureNation) — nema WorldId, briše se kad nestane bilo koja
    // strana. Svaki FK cilja drugu tablicu pa nema "multiple cascade paths" sudara.
    public class ReferenceConfiguration : IEntityTypeConfiguration<Reference>
    {
        public void Configure(EntityTypeBuilder<Reference> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Note).HasMaxLength(1000);

            // Content/Chapter/Episode su Restrict, ne Cascade: Contents->Chapters (Cascade) i
            // Contents->Episodes (Cascade) bi zajedno s direktnim Contents->References Cascade-om
            // stvorili TRI konvergentna cascade puta na References (SQL Server "multiple cascade
            // paths"). Repozitoriji ručno brišu pogođene References retke prije brisanja
            // Content/Chapter/Episode (ContentRepository.DeleteAsync, WorldRepository.DeleteAsync).
            builder.HasOne(r => r.Content)
                .WithMany(c => c.References)
                .HasForeignKey(r => r.ContentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Chapter)
                .WithMany(c => c.References)
                .HasForeignKey(r => r.ChapterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Episode)
                .WithMany(e => e.References)
                .HasForeignKey(r => r.EpisodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Character)
                .WithMany()
                .HasForeignKey(r => r.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Location)
                .WithMany()
                .HasForeignKey(r => r.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Faction)
                .WithMany()
                .HasForeignKey(r => r.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Nation)
                .WithMany()
                .HasForeignKey(r => r.NationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => r.ContentId);
            builder.HasIndex(r => r.ChapterId);
            builder.HasIndex(r => r.EpisodeId);
            builder.HasIndex(r => r.CharacterId);
            builder.HasIndex(r => r.LocationId);
            builder.HasIndex(r => r.FactionId);
            builder.HasIndex(r => r.NationId);
        }
    }
}
