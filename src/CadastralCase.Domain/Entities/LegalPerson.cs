namespace CadastralCase.Domain.Entities;

public class LegalPerson : EntityBase
{
    public string CompanyName { get; private set; }
    public string TradeName { get; private set; }
    public string TaxId { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public DateTime FoundingDate { get; private set; }
    
    public Guid? AddressId { get; private set; }
    public Address? Address { get; private set; }

    private LegalPerson() { }

    public LegalPerson(
        string companyName,
        string tradeName,
        string taxId,
        DateTime foundingDate,
        string? email = null,
        string? phone = null)
    {
        ValidateAndSet(companyName, tradeName, taxId, foundingDate, email, phone);
    }

    public void Update(
        string companyName,
        string tradeName,
        DateTime foundingDate,
        string? email = null,
        string? phone = null)
    {
        CompanyName = companyName;
        TradeName = tradeName;
        FoundingDate = foundingDate;
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
        string companyName,
        string tradeName,
        string taxId,
        DateTime foundingDate,
        string? email,
        string? phone)
    {
        CompanyName = companyName;
        TradeName = tradeName;
        TaxId = taxId.Replace(".", "").Replace("-", "").Replace("/", "");
        FoundingDate = foundingDate;
        Email = email;
        Phone = phone;
    }
}
