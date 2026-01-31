using CadastralCase.Domain.Entities;

namespace CadastralCase.Domain.Interfaces;

public interface INaturalPersonRepository : IRepository<NaturalPerson>
{
    Task<NaturalPerson?> GetByTaxIdAsync(string taxId);
    Task<IEnumerable<NaturalPerson>> GetByNameAsync(string name);
}
