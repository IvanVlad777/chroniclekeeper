using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken = default)
        {
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync(cancellationToken);
            return currency;
        }

        public async Task<Currency?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Currencies
                .Include(c => c.History)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Currency?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Currencies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Currency>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Currencies.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Currency> UpdateAsync(Currency currency, CancellationToken cancellationToken = default)
        {
            _context.Entry(currency).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return currency;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Currencies
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountBankingSystemsUsingCurrencyAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            return await _context.BankingSystems
                .CountAsync(b => b.CurrencyId == currencyId, cancellationToken);
        }
    }

    public class BankingSystemRepository : IBankingSystemRepository
    {
        private readonly ApplicationDbContext _context;

        public BankingSystemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BankingSystem> CreateAsync(BankingSystem bankingSystem, CancellationToken cancellationToken = default)
        {
            _context.BankingSystems.Add(bankingSystem);
            await _context.SaveChangesAsync(cancellationToken);
            return bankingSystem;
        }

        public async Task<BankingSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.BankingSystems
                .Include(b => b.Currency)
                .Include(b => b.History)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<BankingSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.BankingSystems
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<List<BankingSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.BankingSystems.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(b => b.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<BankingSystem> UpdateAsync(BankingSystem bankingSystem, CancellationToken cancellationToken = default)
        {
            _context.Entry(bankingSystem).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return bankingSystem;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.BankingSystems
                .Where(b => b.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountEconomicSystemsUsingBankingSystemAsync(int bankingSystemId, CancellationToken cancellationToken = default)
        {
            return await _context.EconomicSystems
                .CountAsync(e => e.BankingSystemId == bankingSystemId, cancellationToken);
        }

        public async Task<int> CountCorporationsUsingBankingSystemAsync(int bankingSystemId, CancellationToken cancellationToken = default)
        {
            return await _context.Corporations
                .CountAsync(c => c.BankingSystemId == bankingSystemId, cancellationToken);
        }
    }

    public class TaxationSystemRepository : ITaxationSystemRepository
    {
        private readonly ApplicationDbContext _context;

        public TaxationSystemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaxationSystem> CreateAsync(TaxationSystem taxationSystem, CancellationToken cancellationToken = default)
        {
            _context.TaxationSystems.Add(taxationSystem);
            await _context.SaveChangesAsync(cancellationToken);
            return taxationSystem;
        }

        public async Task<TaxationSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.TaxationSystems
                .Include(t => t.History)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<TaxationSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.TaxationSystems
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TaxationSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.TaxationSystems.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(t => t.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TaxationSystem> UpdateAsync(TaxationSystem taxationSystem, CancellationToken cancellationToken = default)
        {
            _context.Entry(taxationSystem).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return taxationSystem;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.TaxationSystems
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountEconomicSystemsUsingTaxationSystemAsync(int taxationSystemId, CancellationToken cancellationToken = default)
        {
            return await _context.EconomicSystems
                .CountAsync(e => e.TaxationSystemId == taxationSystemId, cancellationToken);
        }

        public async Task<int> CountGuildsUsingTaxationSystemAsync(int taxationSystemId, CancellationToken cancellationToken = default)
        {
            return await _context.Guilds
                .CountAsync(g => g.TaxationSystemId == taxationSystemId, cancellationToken);
        }

        public async Task<int> CountCorporationsUsingTaxationSystemAsync(int taxationSystemId, CancellationToken cancellationToken = default)
        {
            return await _context.Corporations
                .CountAsync(c => c.TaxationSystemId == taxationSystemId, cancellationToken);
        }
    }

    public class EconomicSystemRepository : IEconomicSystemRepository
    {
        private readonly ApplicationDbContext _context;

        public EconomicSystemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EconomicSystem> CreateAsync(EconomicSystem economicSystem, CancellationToken cancellationToken = default)
        {
            _context.EconomicSystems.Add(economicSystem);
            await _context.SaveChangesAsync(cancellationToken);
            return economicSystem;
        }

        public async Task<EconomicSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.EconomicSystems
                .Include(e => e.TaxationSystem)
                .Include(e => e.BankingSystem)
                .Include(e => e.History)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<EconomicSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.EconomicSystems
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<EconomicSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.EconomicSystems.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(e => e.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<EconomicSystem> UpdateAsync(EconomicSystem economicSystem, CancellationToken cancellationToken = default)
        {
            _context.Entry(economicSystem).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return economicSystem;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.EconomicSystems
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<int> CountLocationsUsingEconomicSystemAsync(int economicSystemId, CancellationToken cancellationToken = default)
        {
            var countries = await _context.Countries
                .CountAsync(c => c.EconomicSystemId == economicSystemId, cancellationToken);
            var cities = await _context.Cities
                .CountAsync(c => c.EconomicSystemId == economicSystemId, cancellationToken);
            return countries + cities;
        }
    }
}
