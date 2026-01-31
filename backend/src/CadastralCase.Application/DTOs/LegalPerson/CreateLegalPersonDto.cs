namespace CadastralCase.Application.DTOs.LegalPerson;

public record CreateLegalPersonDto
{
    public string CompanyName { get; init; } = string.Empty;
    public string TradeName { get; init; } = string.Empty;
    public string TaxId { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public DateTime FoundingDate { get; init; }
    public Guid? AddressId { get; init; }
}
