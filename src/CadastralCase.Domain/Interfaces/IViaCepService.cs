using CadastralCase.Domain.Models;

namespace CadastralCase.Domain.Interfaces;

public interface IViaCepService
{
    Task<ViaCepResponse?> GetAddressAsync(string postalCode);
}
