namespace CadastralCase.Domain.Models;

public class ViaCepResponse
{
    public string PostalCode { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Complement { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string StateName { get; set; } = string.Empty;
    public string IbgeCode { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty;
    public bool Error { get; set; }
}