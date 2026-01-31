namespace CadastralCase.Application.DTOs.Address;

public record CreateAddressDto
{
    public string PostalCode { get; init; } = string.Empty;
    public string? Street { get; init; }
    public string? Complement { get; init; }
    public string? Number { get; init; }
    public string? District { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? StateName { get; init; }
    public string? IbgeCode { get; init; }
    public string? AreaCode { get; init; }
    public bool QueryViaCep { get; init; } = true;
}
