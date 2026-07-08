using ChronicleKeeper.Core.Entities.Notes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class NoteConfiguration : LoreEntityConfiguration<Note>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Note> builder)
        {
            // Content ostaje nvarchar(max) — bez ograničenja duljine
        }
    }
}
