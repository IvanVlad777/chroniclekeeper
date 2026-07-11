using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Entities.Content.Article;
using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Entities.Content.Movie;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    /// <summary>
    /// TPH root — Article/Book/Comic/Movie/Series share this "Contents" table,
    /// distinguished by the "ContentType" string discriminator (mirrors School/TradeSchool).
    /// </summary>
    public class ContentConfiguration : LoreEntityConfiguration<Content>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Content> builder)
        {
            builder.HasDiscriminator<string>("ContentType")
                .HasValue<Article>("Article")
                .HasValue<Book>("Book")
                .HasValue<Comic>("Comic")
                .HasValue<Movie>("Movie")
                .HasValue<Series>("Series");
        }
    }

    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(a => a.Source).HasMaxLength(200);
        }
    }

    public class ComicConfiguration : IEntityTypeConfiguration<Comic>
    {
        public void Configure(EntityTypeBuilder<Comic> builder)
        {
            builder.Property(c => c.Author).HasMaxLength(100);
        }
    }

    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Author).HasMaxLength(100);
        }
    }

    public class ChapterConfiguration : LoreEntityConfiguration<Chapter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Chapter> builder)
        {
            // Compositional child of Book, required FK — same shape as JobRank→Profession
            builder.HasOne(c => c.Book)
                .WithMany(b => b.Chapters)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            builder.Property(s => s.Creator).HasMaxLength(100);
        }
    }

    public class EpisodeConfiguration : LoreEntityConfiguration<Episode>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Episode> builder)
        {
            // Compositional child of Series, required FK — same shape as JobRank→Profession
            builder.HasOne(e => e.Series)
                .WithMany(s => s.Episodes)
                .HasForeignKey(e => e.SeriesId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(m => m.Director).HasMaxLength(100);

            // Self-ref hijerarhija — Restrict: brisanje prequela sa sequelima je friendly app greška
            builder.HasOne(m => m.Prequel)
                .WithMany(m => m.Sequels)
                .HasForeignKey(m => m.PrequelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => m.PrequelId);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Movies_Prequel_NotSelf", "[PrequelId] <> [Id]"));
        }
    }
}
