using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Content;
using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Entities.Content.Article;
using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Entities.Content.Movie;

namespace ChronicleKeeperAPI.Mapping.Contents
{
    /// <summary>
    /// Content is an abstract TPH root (Article/Book/Comic/Movie/Series). AutoMapper can map a
    /// polymorphic source into one flat destination DTO fine — each ForMember below delegates to a
    /// plain static helper that pattern-matches on the runtime type to pick the right type-specific
    /// field(s), leaving the rest null. The pattern-matching has to live in an ordinary method body
    /// (not inlined in the MapFrom lambda) because MapFrom's single-parameter overload compiles its
    /// lambda as an expression tree, and C# 'is' pattern matching isn't allowed inside those (CS8122).
    /// </summary>
    public class ContentProfile : Profile
    {
        public ContentProfile()
        {
            CreateMap<Content, ContentDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => GetTypeName(src)))
                .ForMember(d => d.Source, opt => opt.MapFrom(src => GetSource(src)))
                .ForMember(d => d.PublishDate, opt => opt.MapFrom(src => GetPublishDate(src)))
                .ForMember(d => d.Author, opt => opt.MapFrom(src => GetAuthor(src)))
                .ForMember(d => d.ReleaseDate, opt => opt.MapFrom(src => GetReleaseDate(src)))
                .ForMember(d => d.IssueNumber, opt => opt.MapFrom(src => GetIssueNumber(src)))
                .ForMember(d => d.Director, opt => opt.MapFrom(src => GetDirector(src)))
                .ForMember(d => d.DurationMinutes, opt => opt.MapFrom(src => GetDurationMinutes(src)))
                .ForMember(d => d.PrequelId, opt => opt.MapFrom(src => GetPrequelId(src)))
                .ForMember(d => d.Creator, opt => opt.MapFrom(src => GetCreator(src)))
                .ForMember(d => d.Seasons, opt => opt.MapFrom(src => GetSeasons(src)));

            CreateMap<Content, ContentDetailsDto>()
                .IncludeBase<Content, ContentDto>()
                .ForMember(d => d.Chapters, opt => opt.MapFrom(src => GetChapters(src)))
                .ForMember(d => d.Episodes, opt => opt.MapFrom(src => GetEpisodes(src)))
                .ForMember(d => d.Prequel, opt => opt.MapFrom(src => GetPrequel(src)))
                .ForMember(d => d.Sequels, opt => opt.MapFrom(src => GetSequels(src)))
                .ForMember(d => d.References, opt => opt.MapFrom(src => BuildReferenceEntries(src)));
        }

        private static string GetTypeName(Content content) => content switch
        {
            Article => "Article",
            Book => "Book",
            Comic => "Comic",
            Movie => "Movie",
            Series => "Series",
            _ => content.GetType().Name
        };

        private static string? GetSource(Content content) => content is Article a ? a.Source : null;

        private static DateTime? GetPublishDate(Content content) => content is Article a ? a.PublishDate : null;

        private static string? GetAuthor(Content content) => content switch
        {
            Book b => b.Author,
            Comic c => c.Author,
            _ => null
        };

        private static DateTime? GetReleaseDate(Content content) => content switch
        {
            Book b => b.ReleaseDate,
            Movie m => m.ReleaseDate,
            _ => null
        };

        private static int? GetIssueNumber(Content content) => content is Comic c ? c.IssueNumber : null;

        private static string? GetDirector(Content content) => content is Movie m ? m.Director : null;

        private static int? GetDurationMinutes(Content content) => content is Movie m ? m.DurationMinutes : null;

        private static int? GetPrequelId(Content content) => content is Movie m ? m.PrequelId : null;

        private static string? GetCreator(Content content) => content is Series s ? s.Creator : null;

        private static int? GetSeasons(Content content) => content is Series s ? s.Seasons : null;

        private static List<ReferenceDto> GetChapters(Content content) => content is Book b
            ? b.Chapters.OrderBy(ch => ch.Order).Select(ch => new ReferenceDto { Id = ch.Id, Name = ch.Name }).ToList()
            : new List<ReferenceDto>();

        private static List<ReferenceDto> GetEpisodes(Content content) => content is Series s
            ? s.Episodes.OrderBy(e => e.Season).ThenBy(e => e.Order).Select(e => new ReferenceDto { Id = e.Id, Name = e.Name }).ToList()
            : new List<ReferenceDto>();

        private static ReferenceDto? GetPrequel(Content content) => content is Movie { Prequel: not null } m
            ? new ReferenceDto { Id = m.Prequel!.Id, Name = m.Prequel.Name }
            : null;

        private static List<ReferenceDto> GetSequels(Content content) => content is Movie m
            ? m.Sequels.Select(sq => new ReferenceDto { Id = sq.Id, Name = sq.Name }).ToList()
            : new List<ReferenceDto>();

        private static List<ContentReferenceEntryDto> BuildReferenceEntries(Content content)
        {
            var result = new List<ContentReferenceEntryDto>();
            foreach (var r in content.References)
            {
                if (r.Character != null)
                {
                    result.Add(new ContentReferenceEntryDto { Id = r.Id, Note = r.Note, EntityType = "Character", EntityId = r.Character.Id, EntityName = r.Character.Name });
                }
                else if (r.Location != null)
                {
                    result.Add(new ContentReferenceEntryDto { Id = r.Id, Note = r.Note, EntityType = "Location", EntityId = r.Location.Id, EntityName = r.Location.Name });
                }
                else if (r.Faction != null)
                {
                    result.Add(new ContentReferenceEntryDto { Id = r.Id, Note = r.Note, EntityType = "Faction", EntityId = r.Faction.Id, EntityName = r.Faction.Name });
                }
                else if (r.Nation != null)
                {
                    result.Add(new ContentReferenceEntryDto { Id = r.Id, Note = r.Note, EntityType = "Nation", EntityId = r.Nation.Id, EntityName = r.Nation.Name });
                }
                // else: none of the 4 world-entity FKs set on this row — shouldn't happen, skip defensively.
            }
            return result;
        }
    }
}
