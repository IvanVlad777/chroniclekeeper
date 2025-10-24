using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ApplicationDbContext _context;

        public CharacterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Character> CreateAsync(Character character, CancellationToken cancellationToken = default)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync(cancellationToken);
            return character;
        }

        public async Task<Character?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Characters
                // TODO: Otkomentirati kada budem dodavao veze
                //.Include(c => c.SapientSpecies)
                //.Include(c => c.Religion)
                //.Include(c => c.Nation)
                //.Include(c => c.Profession)
                //.Include(c => c.SocialClass)
                //.Include(c => c.Father)
                //.Include(c => c.Mother)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Character>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Characters
                // TODO: Otkomentirati kada budem dodavao veze
                //.Include(c => c.SapientSpecies)
                //.Include(c => c.Religion)
                //.Include(c => c.Nation)
                //.Include(c => c.Profession)
                //.Include(c => c.SocialClass)
                //.Include(c => c.Father)
                //.Include(c => c.Mother)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Character> UpdateAsync(Character character, CancellationToken cancellationToken = default)
        {
            _context.Characters.Update(character);
            await _context.SaveChangesAsync(cancellationToken);
            return character;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var character = await _context.Characters.FindAsync(new object[] { id }, cancellationToken);
            if (character != null)
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
