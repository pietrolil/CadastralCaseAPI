namespace CadastralCase.Application.DTOs.Address;

public record UpdateAddressDto
{
    public string PostalCode { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string? Complement { get; init; }
    public string? Number { get; init; }
    public string? District { get; init; }
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string StateName { get; init; } = string.Empty;
    public string? IbgeCode { get; init; }
    public string? AreaCode { get; init; }
}
