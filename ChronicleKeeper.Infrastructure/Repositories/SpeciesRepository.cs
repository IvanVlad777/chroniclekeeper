using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly ApplicationDbContext _context;

        public SpeciesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ----- Species -----

        public async Task<SapientSpecies> CreateAsync(SapientSpecies species, CancellationToken cancellationToken = default)
        {
            _context.SapientSpecies.Add(species);
            await _context.SaveChangesAsync(cancellationToken);
            return species;
        }

        public async Task<SapientSpecies?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SapientSpecies
                .Include(s => s.Races)
                .Include(s => s.ParentCreature)
                .Include(s => s.Subspecies)
                .Include(s => s.History)
                .Include(s => s.FrequentOccupations).ThenInclude(x => x.Profession)
                .Include(s => s.Cultures).ThenInclude(x => x.Culture)
                .Include(s => s.Folklore).ThenInclude(x => x.Folklore)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<SapientSpecies?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SapientSpecies
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<SapientSpecies>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.SapientSpecies.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(s => s.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<SapientSpecies> UpdateAsync(SapientSpecies species, CancellationToken cancellationToken = default)
        {
            _context.Entry(species).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return species;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Races kaskadira DB; likove je handler već provjerio (Restrict)
            var deleted = await _context.SapientSpecies
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountCharactersUsingSpeciesAsync(int speciesId, CancellationToken cancellationToken = default)
        {
            return await _context.Characters
                .CountAsync(c => c.SapientSpeciesId == speciesId
                    || (c.Race != null && c.Race.SapientSpeciesId == speciesId), cancellationToken);
        }

        // ----- Races -----

        public async Task<Race> CreateRaceAsync(Race race, CancellationToken cancellationToken = default)
        {
            _context.Races.Add(race);
            await _context.SaveChangesAsync(cancellationToken);
            return race;
        }

        public async Task<Race?> FindRaceByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Races
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<Race>> GetRacesAsync(int? worldId = null, int? speciesId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Races.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(r => r.WorldId == wid);
            }
            if (speciesId is int sid)
            {
                query = query.Where(r => r.SapientSpeciesId == sid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Race> UpdateRaceAsync(Race race, CancellationToken cancellationToken = default)
        {
            _context.Entry(race).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return race;
        }

        public async Task<bool> DeleteRaceAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Races
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountCharactersUsingRaceAsync(int raceId, CancellationToken cancellationToken = default)
        {
            return await _context.Characters.CountAsync(c => c.RaceId == raceId, cancellationToken);
        }
    }
}
