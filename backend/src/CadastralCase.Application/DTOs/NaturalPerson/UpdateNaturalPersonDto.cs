namespace CadastralCase.Application.DTOs.NaturalPerson;

public record UpdateNaturalPersonDto
{
    public string Name { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public DateTime BirthDate { get; init; }
    public Guid? AddressId { get; init; }
}
