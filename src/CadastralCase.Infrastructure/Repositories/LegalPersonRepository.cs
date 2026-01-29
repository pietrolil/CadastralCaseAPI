using CadastralCase.Domain.Entities;
using CadastralCase.Domain.Interfaces;
using CadastralCase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CadastralCase.Infrastructure.Repositories;

public class LegalPersonRepository : ILegalPersonRepository
{
    private readonly ApplicationDbContext _context;

    public LegalPersonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LegalPerson?> GetByIdAsync(Guid id)
    {
        return await _context.LegalPersons
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<LegalPerson>> GetAllAsync()
    {
        return await _context.LegalPersons
            .ToListAsync();
    }

    public async Task<IEnumerable<LegalPerson>> GetActiveAsync()
    {
        return await _context.LegalPersons
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<LegalPerson?> GetByTaxIdAsync(string taxId)
    {
        var cleanTaxId = taxId.Replace(".", "").Replace("/", "").Replace("-", "");
        return await _context.LegalPersons
            .FirstOrDefaultAsync(p => p.TaxId == cleanTaxId);
    }

    public async Task<IEnumerable<LegalPerson>> GetByCompanyNameAsync(string companyName)
    {
        return await _context.LegalPersons
            .Where(p => p.CompanyName.Contains(companyName))
            .ToListAsync();
    }

    public async Task<LegalPerson> AddAsync(LegalPerson entity)
    {
        await _context.LegalPersons.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(LegalPerson entity)
    {
        _context.LegalPersons.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.LegalPersons.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ActivateAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.Activate();
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeactivateAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.Deactivate();
            await _context.SaveChangesAsync();
        }
    }
}
