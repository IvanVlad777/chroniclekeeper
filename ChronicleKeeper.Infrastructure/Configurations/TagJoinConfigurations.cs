using ChronicleKeeper.Core.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Join tablice smiju kaskadirati s obje strane: entitet i Tag nemaju
    // zajedničkog kaskadirajućeg pretka (WorldId je Restrict).

    public class CharacterTagConfiguration : IEntityTypeConfiguration<CharacterTag>
    {
        public void Configure(EntityTypeBuilder<CharacterTag> builder)
        {
            builder.HasKey(ct => new { ct.CharacterId, ct.TagId });

            builder.HasOne(ct => ct.Character)
                .WithMany(c => c.Tags)
                .HasForeignKey(ct => ct.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ct => ct.Tag)
                .WithMany()
                .HasForeignKey(ct => ct.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ct => ct.TagId);
        }
    }

    public class LocationTagConfiguration : IEntityTypeConfiguration<LocationTag>
    {
        public void Configure(EntityTypeBuilder<LocationTag> builder)
        {
            builder.HasKey(lt => new { lt.LocationId, lt.TagId });

            builder.HasOne(lt => lt.Location)
                .WithMany(l => l.Tags)
                .HasForeignKey(lt => lt.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lt => lt.Tag)
                .WithMany()
                .HasForeignKey(lt => lt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(lt => lt.TagId);
        }
    }

    public class FactionTagConfiguration : IEntityTypeConfiguration<FactionTag>
    {
        public void Configure(EntityTypeBuilder<FactionTag> builder)
        {
            builder.HasKey(ft => new { ft.FactionId, ft.TagId });

            builder.HasOne(ft => ft.Faction)
                .WithMany(f => f.Tags)
                .HasForeignKey(ft => ft.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ft => ft.Tag)
                .WithMany()
                .HasForeignKey(ft => ft.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ft => ft.TagId);
        }
    }
}
