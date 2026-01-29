namespace CadastralCase.Domain.Entities;

public class NaturalPerson : EntityBase
{
    public string Name { get; private set; }
    public string TaxId { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public DateTime BirthDate { get; private set; }
    
    public Guid? AddressId { get; private set; }
    public Address? Address { get; private set; }

    private NaturalPerson() { }

    public NaturalPerson(
        string name,
        string taxId,
        DateTime birthDate,
        string? email = null,
        string? phone = null)
    {
        ValidateAndSet(name, taxId, birthDate, email, phone);
    }

    public void Update(
        string name,
        DateTime birthDate,
        string? email = null,
        string? phone = null)
    {
        Name = name;
        BirthDate = birthDate;
        Email = email;
        Phone = phone;
        SetUpdatedAt();
    }

    public void SetAddress(Guid addressId)
    {
        AddressId = addressId;
        SetUpdatedAt();
    }

    public void RemoveAddress()
    {
        AddressId = null;
        Address = null;
        SetUpdatedAt();
    }

    private void ValidateAndSet(
        string name,
        string taxId,
        DateTime birthDate,
        string? email,
        string? phone)
    {
        Name = name;
        TaxId = taxId.Replace(".", "").Replace("-", "");
        BirthDate = birthDate;
        Email = email;
        Phone = phone;
    }
}
