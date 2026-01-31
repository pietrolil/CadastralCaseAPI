namespace CadastralCase.Domain.Entities;

public class Address : EntityBase
{
    public string PostalCode { get; private set; }
    public string Street { get; private set; }
    public string? Complement { get; private set; }
    public string? Number { get; private set; }
    public string? District { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string StateName { get; private set; }
    public string? IbgeCode { get; private set; }
    public string? AreaCode { get; private set; }

    private Address() { }

    public Address(
        string postalCode,
        string street,
        string city,
        string state,
        string stateName,
        string? complement = null,
        string? number = null,
        string? district = null,
        string? ibgeCode = null,
        string? areaCode = null)
    {
        ValidateAndSet(postalCode, street, city, state, stateName, complement, number, district, ibgeCode, areaCode);
    }

    public void Update(
        string street,
        string city,
        string state,
        string stateName,
        string? complement = null,
        string? number = null,
        string? district = null,
        string? ibgeCode = null,
        string? areaCode = null)
    {
        Street = street;
        City = city;
        State = state.ToUpper();
        StateName = stateName;
        Complement = complement;
        Number = number;
        District = district;
        IbgeCode = ibgeCode;
        AreaCode = areaCode;
        SetUpdatedAt();
    }

    private void ValidateAndSet(
        string postalCode,
        string street,
        string city,
        string state,
        string stateName,
        string? complement,
        string? number,
        string? district,
        string? ibgeCode,
        string? areaCode)
    {
        PostalCode = postalCode.Replace("-", "");
        Street = street;
        City = city;
        State = state.ToUpper();
        StateName = stateName;
        Complement = complement;
        Number = number;
        District = district;
        IbgeCode = ibgeCode;
        AreaCode = areaCode;
    }
}
