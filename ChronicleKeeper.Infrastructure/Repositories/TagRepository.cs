using ChronicleKeeper.Core.DTOs.Tag;
using ChronicleKeeper.Core.Entities.Tags;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> CreateAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync(cancellationToken);
            return tag;
        }

        public async Task<Tag?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<Tag>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Tags.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(t => t.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Tag> UpdateAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return tag;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Join tablice kaskadira DB
            var deleted = await _context.Tags
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> NameExistsInWorldAsync(string name, int worldId, int? excludeTagId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Tags.Where(t => t.WorldId == worldId && t.Name == name);
            if (excludeTagId is int excludeId)
            {
                query = query.Where(t => t.Id != excludeId);
            }
            return await query.AnyAsync(cancellationToken);
        }

        public async Task<bool> TargetExistsInWorldAsync(TagTargetType targetType, int targetId, int worldId, CancellationToken cancellationToken = default)
        {
            return targetType switch
            {
                TagTargetType.Character => await _context.Characters.AnyAsync(c => c.Id == targetId && c.WorldId == worldId, cancellationToken),
                TagTargetType.Location => await _context.Locations.AnyAsync(l => l.Id == targetId && l.WorldId == worldId, cancellationToken),
                TagTargetType.Faction => await _context.Factions.AnyAsync(f => f.Id == targetId && f.WorldId == worldId, cancellationToken),
                _ => false
            };
        }

        public async Task<bool> IsAttachedAsync(int tagId, TagTargetType targetType, int targetId, CancellationToken cancellationToken = default)
        {
            return targetType switch
            {
                TagTargetType.Character => await _context.CharacterTags.AnyAsync(t => t.TagId == tagId && t.CharacterId == targetId, cancellationToken),
                TagTargetType.Location => await _context.LocationTags.AnyAsync(t => t.TagId == tagId && t.LocationId == targetId, cancellationToken),
                TagTargetType.Faction => await _context.FactionTags.AnyAsync(t => t.TagId == tagId && t.FactionId == targetId, cancellationToken),
                _ => false
            };
        }

        public async Task AttachAsync(int tagId, TagTargetType targetType, int targetId, CancellationToken cancellationToken = default)
        {
            switch (targetType)
            {
                case TagTargetType.Character:
                    _context.CharacterTags.Add(new CharacterTag { TagId = tagId, CharacterId = targetId });
                    break;
                case TagTargetType.Location:
                    _context.LocationTags.Add(new LocationTag { TagId = tagId, LocationId = targetId });
                    break;
                case TagTargetType.Faction:
                    _context.FactionTags.Add(new FactionTag { TagId = tagId, FactionId = targetId });
                    break;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DetachAsync(int tagId, TagTargetType targetType, int targetId, CancellationToken cancellationToken = default)
        {
            var deleted = targetType switch
            {
                TagTargetType.Character => await _context.CharacterTags
                    .Where(t => t.TagId == tagId && t.CharacterId == targetId)
                    .ExecuteDeleteAsync(cancellationToken),
                TagTargetType.Location => await _context.LocationTags
                    .Where(t => t.TagId == tagId && t.LocationId == targetId)
                    .ExecuteDeleteAsync(cancellationToken),
                TagTargetType.Faction => await _context.FactionTags
                    .Where(t => t.TagId == tagId && t.FactionId == targetId)
                    .ExecuteDeleteAsync(cancellationToken),
                _ => 0
            };
            return deleted > 0;
        }
    }
}
