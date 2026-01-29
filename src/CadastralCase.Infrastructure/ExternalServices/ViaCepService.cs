using CadastralCase.Domain.Interfaces;
using System.Text.Json;
using CadastralCase.Domain.Models;

namespace CadastralCase.Infrastructure.ExternalServices;

/// <summary>
/// Service for integrating with ViaCEP API for address lookup
/// Implements Adapter pattern for external service integration
/// </summary>
public class ViaCepService : IViaCepService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://viacep.com.br";

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ViaCepResponse?> GetAddressAsync(string postalCode)
    {
        try
        {
            var cleanCode = postalCode.Replace("-", "").Replace(".", "").Trim();

            if (string.IsNullOrWhiteSpace(cleanCode) || cleanCode.Length != 8)
                return null;

            var response = await _httpClient.GetAsync($"/ws/{cleanCode}/json/");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var viaCepData = JsonSerializer.Deserialize<ViaCepApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (viaCepData == null)
                return null;

            if (viaCepData.Erro)
            {
                return new ViaCepResponse { Error = true };
            }

            return new ViaCepResponse
            {
                PostalCode = viaCepData.Cep ?? cleanCode,
                Street = viaCepData.Logradouro ?? string.Empty,
                Complement = viaCepData.Complemento ?? string.Empty,
                District = viaCepData.Bairro ?? string.Empty,
                City = viaCepData.Localidade ?? string.Empty,
                State = viaCepData.Uf ?? string.Empty,
                StateName = viaCepData.Estado ?? GetStateName(viaCepData.Uf ?? string.Empty),
                IbgeCode = viaCepData.Ibge ?? string.Empty,
                AreaCode = viaCepData.Ddd ?? string.Empty,
                Error = false
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static string GetStateName(string stateCode)
    {
        return stateCode.ToUpper() switch
        {
            "AC" => "Acre",
            "AL" => "Alagoas",
            "AP" => "Amapá",
            "AM" => "Amazonas",
            "BA" => "Bahia",
            "CE" => "Ceará",
            "DF" => "Distrito Federal",
            "ES" => "Espírito Santo",
            "GO" => "Goiás",
            "MA" => "Maranhão",
            "MT" => "Mato Grosso",
            "MS" => "Mato Grosso do Sul",
            "MG" => "Minas Gerais",
            "PA" => "Pará",
            "PB" => "Paraíba",
            "PR" => "Paraná",
            "PE" => "Pernambuco",
            "PI" => "Piauí",
            "RJ" => "Rio de Janeiro",
            "RN" => "Rio Grande do Norte",
            "RS" => "Rio Grande do Sul",
            "RO" => "Rondônia",
            "RR" => "Roraima",
            "SC" => "Santa Catarina",
            "SP" => "São Paulo",
            "SE" => "Sergipe",
            "TO" => "Tocantins",
            _ => string.Empty
        };
    }
}
