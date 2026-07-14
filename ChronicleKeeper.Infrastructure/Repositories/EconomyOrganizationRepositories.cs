using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class GuildRepository : IGuildRepository
    {
        private readonly ApplicationDbContext _context;

        public GuildRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guild> CreateAsync(Guild guild, CancellationToken cancellationToken = default)
        {
            _context.Guilds.Add(guild);
            await _context.SaveChangesAsync(cancellationToken);
            return guild;
        }

        public async Task<Guild?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Guilds
                .Include(g => g.TaxationSystem)
                .Include(g => g.Industry)
                .Include(g => g.LegalSystem)
                .Include(g => g.EducationSystem)
                .Include(g => g.History)
                .Include(g => g.GuildRanks)
                .Include(g => g.Factions).ThenInclude(gf => gf.Faction)
                .Include(g => g.MemberProfessions).ThenInclude(gp => gp.Profession)
                .Include(g => g.SocialClasses).ThenInclude(gs => gs.SocialClass)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<Guild?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Guilds
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<List<Guild>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Guilds.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(g => g.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Guild> UpdateAsync(Guild guild, CancellationToken cancellationToken = default)
        {
            _context.Entry(guild).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return guild;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Guilds
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsFactionLinkedAsync(int guildId, int factionId, CancellationToken cancellationToken = default)
        {
            return await _context.GuildFactions
                .AnyAsync(gf => gf.GuildId == guildId && gf.FactionId == factionId, cancellationToken);
        }

        public async Task AddFactionAsync(int guildId, int factionId, CancellationToken cancellationToken = default)
        {
            _context.GuildFactions.Add(new GuildFaction { GuildId = guildId, FactionId = factionId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveFactionAsync(int guildId, int factionId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.GuildFactions
                .Where(gf => gf.GuildId == guildId && gf.FactionId == factionId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsProfessionLinkedAsync(int guildId, int professionId, CancellationToken cancellationToken = default)
        {
            return await _context.GuildProfessions
                .AnyAsync(gp => gp.GuildId == guildId && gp.ProfessionId == professionId, cancellationToken);
        }

        public async Task AddProfessionAsync(int guildId, int professionId, CancellationToken cancellationToken = default)
        {
            _context.GuildProfessions.Add(new GuildProfession { GuildId = guildId, ProfessionId = professionId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveProfessionAsync(int guildId, int professionId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.GuildProfessions
                .Where(gp => gp.GuildId == guildId && gp.ProfessionId == professionId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsSocialClassLinkedAsync(int guildId, int socialClassId, CancellationToken cancellationToken = default)
        {
            return await _context.GuildSocialClasses
                .AnyAsync(gs => gs.GuildId == guildId && gs.SocialClassId == socialClassId, cancellationToken);
        }

        public async Task AddSocialClassAsync(int guildId, int socialClassId, CancellationToken cancellationToken = default)
        {
            _context.GuildSocialClasses.Add(new GuildSocialClass { GuildId = guildId, SocialClassId = socialClassId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveSocialClassAsync(int guildId, int socialClassId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.GuildSocialClasses
                .Where(gs => gs.GuildId == guildId && gs.SocialClassId == socialClassId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }

    public class GuildRankRepository : IGuildRankRepository
    {
        private readonly ApplicationDbContext _context;

        public GuildRankRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GuildRank> CreateAsync(GuildRank guildRank, CancellationToken cancellationToken = default)
        {
            _context.GuildRanks.Add(guildRank);
            await _context.SaveChangesAsync(cancellationToken);
            return guildRank;
        }

        public async Task<List<GuildRank>> GetAllAsync(int? worldId = null, int? guildId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.GuildRanks.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(r => r.WorldId == wid);
            }
            if (guildId is int gid)
            {
                query = query.Where(r => r.GuildId == gid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<GuildRank?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.GuildRanks
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<GuildRank> UpdateAsync(GuildRank guildRank, CancellationToken cancellationToken = default)
        {
            _context.Entry(guildRank).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return guildRank;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.GuildRanks
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }

    public class CorporationRepository : ICorporationRepository
    {
        private readonly ApplicationDbContext _context;

        public CorporationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Corporation> CreateAsync(Corporation corporation, CancellationToken cancellationToken = default)
        {
            _context.Corporations.Add(corporation);
            await _context.SaveChangesAsync(cancellationToken);
            return corporation;
        }

        public async Task<Corporation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Corporations
                .Include(c => c.Industry)
                .Include(c => c.TaxationSystem)
                .Include(c => c.BankingSystem)
                .Include(c => c.ParentCorporation)
                .Include(c => c.Subsidiaries)
                .Include(c => c.History)
                .Include(c => c.Leadership).ThenInclude(l => l.Profession)
                .Include(c => c.Leadership).ThenInclude(l => l.Character)
                .Include(c => c.Factions).ThenInclude(cf => cf.Faction)
                .Include(c => c.MemberProfessions).ThenInclude(cp => cp.Profession)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Corporation?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Corporations
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Corporation>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Corporations.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Corporation> UpdateAsync(Corporation corporation, CancellationToken cancellationToken = default)
        {
            _context.Entry(corporation).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return corporation;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Corporations
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> HasSubsidiariesAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Corporations
                .AnyAsync(c => c.ParentCorporationId == id, cancellationToken);
        }

        public async Task<bool> WouldCreateCycleAsync(int corporationId, int newParentId, CancellationToken cancellationToken = default)
        {
            int? current = newParentId;
            var visited = new HashSet<int>();
            while (current is int cid)
            {
                if (cid == corporationId) return true;
                if (!visited.Add(cid)) return true; // postojeći ciklus u podacima — ne pogoršavaj

                current = await _context.Corporations
                    .Where(c => c.Id == cid)
                    .Select(c => c.ParentCorporationId)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            return false;
        }

        public async Task<bool> IsFactionLinkedAsync(int corporationId, int factionId, CancellationToken cancellationToken = default)
        {
            return await _context.CorporationFactions
                .AnyAsync(cf => cf.CorporationId == corporationId && cf.FactionId == factionId, cancellationToken);
        }

        public async Task AddFactionAsync(int corporationId, int factionId, CancellationToken cancellationToken = default)
        {
            _context.CorporationFactions.Add(new CorporationFaction { CorporationId = corporationId, FactionId = factionId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveFactionAsync(int corporationId, int factionId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CorporationFactions
                .Where(cf => cf.CorporationId == corporationId && cf.FactionId == factionId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsProfessionLinkedAsync(int corporationId, int professionId, CancellationToken cancellationToken = default)
        {
            return await _context.CorporationProfessions
                .AnyAsync(cp => cp.CorporationId == corporationId && cp.ProfessionId == professionId, cancellationToken);
        }

        public async Task AddProfessionAsync(int corporationId, int professionId, CancellationToken cancellationToken = default)
        {
            _context.CorporationProfessions.Add(new CorporationProfession { CorporationId = corporationId, ProfessionId = professionId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveProfessionAsync(int corporationId, int professionId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CorporationProfessions
                .Where(cp => cp.CorporationId == corporationId && cp.ProfessionId == professionId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }

    public class CorporateLeadershipRepository : ICorporateLeadershipRepository
    {
        private readonly ApplicationDbContext _context;

        public CorporateLeadershipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CorporateLeadership> CreateAsync(CorporateLeadership leadership, CancellationToken cancellationToken = default)
        {
            _context.CorporateLeaderships.Add(leadership);
            await _context.SaveChangesAsync(cancellationToken);
            return leadership;
        }

        public async Task<List<CorporateLeadership>> GetAllAsync(int? worldId = null, int? corporationId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.CorporateLeaderships.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(l => l.WorldId == wid);
            }
            if (corporationId is int cid)
            {
                query = query.Where(l => l.CorporationId == cid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<CorporateLeadership?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.CorporateLeaderships
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<CorporateLeadership> UpdateAsync(CorporateLeadership leadership, CancellationToken cancellationToken = default)
        {
            _context.Entry(leadership).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return leadership;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.CorporateLeaderships
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
