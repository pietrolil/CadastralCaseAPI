using CadastralCase.Domain.Entities;

namespace CadastralCase.Domain.Interfaces;

public interface ILegalPersonRepository : IRepository<LegalPerson>
{
    Task<LegalPerson?> GetByTaxIdAsync(string taxId);
    Task<IEnumerable<LegalPerson>> GetByCompanyNameAsync(string companyName);
}
