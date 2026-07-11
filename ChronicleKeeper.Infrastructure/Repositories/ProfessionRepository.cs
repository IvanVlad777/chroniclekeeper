using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ProfessionRepository : IProfessionRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Profession> CreateAsync(Profession profession, CancellationToken cancellationToken = default)
        {
            _context.Professions.Add(profession);
            await _context.SaveChangesAsync(cancellationToken);
            return profession;
        }

        public async Task<Profession?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Professions
                .Include(p => p.JobRanks)
                .Include(p => p.Apprenticeships).ThenInclude(a => a.TradeSchool)
                .Include(p => p.Specialisations)
                .Include(p => p.PracticedBySpecies).ThenInclude(ps => ps.SapientSpecies)
                .Include(p => p.SocialClasses).ThenInclude(ps => ps.SocialClass)
                .Include(p => p.TradeSchools).ThenInclude(pt => pt.TradeSchool)
                .Include(p => p.Characters)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Profession?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Professions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Profession>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Professions.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(p => p.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Profession> UpdateAsync(Profession profession, CancellationToken cancellationToken = default)
        {
            _context.Entry(profession).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return profession;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Professions
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountCharactersUsingProfessionAsync(int professionId, CancellationToken cancellationToken = default)
        {
            return await _context.Characters.CountAsync(c => c.ProfessionId == professionId, cancellationToken);
        }

        public async Task<bool> IsSpeciesLinkedAsync(int professionId, int speciesId, CancellationToken cancellationToken = default)
        {
            return await _context.ProfessionSapientSpecies
                .AnyAsync(ps => ps.ProfessionId == professionId && ps.SapientSpeciesId == speciesId, cancellationToken);
        }

        public async Task AddSpeciesAsync(int professionId, int speciesId, CancellationToken cancellationToken = default)
        {
            _context.ProfessionSapientSpecies.Add(new ProfessionSapientSpecies { ProfessionId = professionId, SapientSpeciesId = speciesId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveSpeciesAsync(int professionId, int speciesId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ProfessionSapientSpecies
                .Where(ps => ps.ProfessionId == professionId && ps.SapientSpeciesId == speciesId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsSocialClassLinkedAsync(int professionId, int socialClassId, CancellationToken cancellationToken = default)
        {
            return await _context.ProfessionSocialClasses
                .AnyAsync(ps => ps.ProfessionId == professionId && ps.SocialClassId == socialClassId, cancellationToken);
        }

        public async Task AddSocialClassAsync(int professionId, int socialClassId, CancellationToken cancellationToken = default)
        {
            _context.ProfessionSocialClasses.Add(new ProfessionSocialClass { ProfessionId = professionId, SocialClassId = socialClassId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveSocialClassAsync(int professionId, int socialClassId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ProfessionSocialClasses
                .Where(ps => ps.ProfessionId == professionId && ps.SocialClassId == socialClassId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsTradeSchoolLinkedAsync(int professionId, int tradeSchoolId, CancellationToken cancellationToken = default)
        {
            return await _context.ProfessionTradeSchools
                .AnyAsync(pt => pt.ProfessionId == professionId && pt.TradeSchoolId == tradeSchoolId, cancellationToken);
        }

        public async Task AddTradeSchoolAsync(int professionId, int tradeSchoolId, CancellationToken cancellationToken = default)
        {
            _context.ProfessionTradeSchools.Add(new ProfessionTradeSchool { ProfessionId = professionId, TradeSchoolId = tradeSchoolId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveTradeSchoolAsync(int professionId, int tradeSchoolId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ProfessionTradeSchools
                .Where(pt => pt.ProfessionId == professionId && pt.TradeSchoolId == tradeSchoolId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
