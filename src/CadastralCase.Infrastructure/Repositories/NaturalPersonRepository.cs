using CadastralCase.Domain.Entities;
using CadastralCase.Domain.Interfaces;
using CadastralCase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CadastralCase.Infrastructure.Repositories;

public class NaturalPersonRepository : INaturalPersonRepository
{
    private readonly ApplicationDbContext _context;

    public NaturalPersonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NaturalPerson?> GetByIdAsync(Guid id)
    {
        return await _context.NaturalPersons
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<NaturalPerson>> GetAllAsync()
    {
        return await _context.NaturalPersons
            .ToListAsync();
    }

    public async Task<IEnumerable<NaturalPerson>> GetActiveAsync()
    {
        return await _context.NaturalPersons
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<NaturalPerson?> GetByTaxIdAsync(string taxId)
    {
        var cleanTaxId = taxId.Replace(".", "").Replace("-", "");
        return await _context.NaturalPersons
            .FirstOrDefaultAsync(p => p.TaxId == cleanTaxId);
    }

    public async Task<IEnumerable<NaturalPerson>> GetByNameAsync(string name)
    {
        return await _context.NaturalPersons
            .Where(p => p.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<NaturalPerson> AddAsync(NaturalPerson entity)
    {
        await _context.NaturalPersons.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(NaturalPerson entity)
    {
        _context.NaturalPersons.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.NaturalPersons.Remove(entity);
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
