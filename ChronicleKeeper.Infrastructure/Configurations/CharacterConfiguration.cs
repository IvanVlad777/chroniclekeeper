using ChronicleKeeper.Core.Entities.Characters;
// using ChronicleKeeper.Core.Entities.Characters.Equipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {   
        // Primarni ključ
        builder.HasKey(c => c.Id);
        
        // Obavezna polja
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(c => c.CreatedAt)
            .IsRequired();
            
        builder.Property(c => c.UpdatedAt)
            .IsRequired();
        
        // Opcionalna polja sa maksimalnim dužinama
        builder.Property(c => c.Description)
            .HasMaxLength(1000);
            
        builder.Property(c => c.LastName)
            .HasMaxLength(50);
            
        builder.Property(c => c.Nickname)
            .HasMaxLength(50);
            
        builder.Property(c => c.Title)
            .HasMaxLength(100);
            
        builder.Property(c => c.HairColor)
            .HasMaxLength(50);
            
        builder.Property(c => c.EyeColor)
            .HasMaxLength(50);
            
        builder.Property(c => c.SpecialPhysicalFeatures)
            .HasMaxLength(500);
        
        // Indeksi za brže pretraživanje
        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.FirstName);
        builder.HasIndex(c => c.LastName);
        
        /*
        // Self-referencing Parent-Child Relationship
        builder.HasOne(c => c.Father)
            .WithMany()
            .HasForeignKey(c => c.FatherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Mother)
            .WithMany()
            .HasForeignKey(c => c.MotherId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sibling Relationship
        builder.HasMany(c => c.Siblings)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
        */
    }
}