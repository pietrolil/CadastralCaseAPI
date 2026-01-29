using CadastralCase.Domain.Entities;

namespace CadastralCase.Domain.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address?> GetByPostalCodeAsync(string postalCode);
    Task<IEnumerable<Address>> GetByCityAsync(string city, string state);
}
