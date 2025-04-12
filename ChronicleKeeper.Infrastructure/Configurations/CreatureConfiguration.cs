using ChronicleKeeper.Core.Entities.Geography.Creatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

public class CreatureConfiguration : IEntityTypeConfiguration<Creature>
{
    public void Configure(EntityTypeBuilder<Creature> builder)
    {
        builder.HasOne(c => c.ParentCreature)
            .WithMany(c => c.Subspecies)
            .HasForeignKey(c => c.ParentCreatureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Prey)
            .WithMany(c => c.Predators)
            .UsingEntity(j => j.ToTable("CreaturePredation"));

        builder.HasMany(c => c.SymbioticPartners)
            .WithMany()
            .UsingEntity(j => j.ToTable("CreatureSymbiosis"));
    }
}