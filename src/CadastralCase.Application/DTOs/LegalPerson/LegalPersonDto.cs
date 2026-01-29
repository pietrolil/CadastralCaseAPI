using CadastralCase.Application.DTOs.Address;

namespace CadastralCase.Application.DTOs.LegalPerson;

public record LegalPersonDto
{
    public Guid Id { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public string TradeName { get; init; } = string.Empty;
    public string TaxId { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public DateTime FoundingDate { get; init; }
    public Guid? AddressId { get; init; }
    public AddressDto? Address { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public bool IsActive { get; init; }
}
