using CadastralCase.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CadastralCase.Tests.Domain;

public class AddressTests
{
    [Fact]
    public void Should_CreateAddress_When_GivenValidData()
    {
        // Arrange
        var PostalCode = "58348000";
        var Street = "Rua Principal";
        var City = "Riachão do Poço";
        var State = "PB";
        var StateName = "Paraíba";
        var Complement = "Casa";
        var Number = "123";
        var District = "Centro";
        var IbgeCode = "2512762";
        var AreaCode = "83";

        // Act
        var Address = new Address(PostalCode, Street, City, State, StateName, 
            Complement, Number, District, IbgeCode, AreaCode);

        // Assert
        Address.Should().NotBeNull();
        Address.PostalCode.Should().Be("58348000");
        Address.Street.Should().Be(Street);
        Address.City.Should().Be(City);
        Address.State.Should().Be("PB");
        Address.StateName.Should().Be(StateName);
        Address.Complement.Should().Be(Complement);
        Address.Number.Should().Be(Number);
        Address.District.Should().Be(District);
        Address.IbgeCode.Should().Be(IbgeCode);
        Address.AreaCode.Should().Be(AreaCode);
        Address.IsActive.Should().BeTrue();
        Address.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_RemoveMask_When_PostalCodeHasMask()
    {
        // Arrange
        var PostalCodeComMascara = "58348-000";
        var Street = "Rua Principal";
        var City = "Riachão do Poço";
        var State = "PB";
        var StateName = "Paraíba";

        // Act
        var Address = new Address(PostalCodeComMascara, Street, City, State, StateName);

        // Assert
        Address.PostalCode.Should().Be("58348000");
    }

    [Fact]
    public void Should_ConvertStateToUppercase_When_StateIsLowercase()
    {
        // Arrange
        var PostalCode = "58348000";
        var Street = "Rua Principal";
        var City = "Riachão do Poço";
        var StateMinuscula = "pb";
        var StateName = "Paraíba";

        // Act
        var Address = new Address(PostalCode, Street, City, StateMinuscula, StateName);

        // Assert
        Address.State.Should().Be("PB");
    }

    [Fact]
    public void Should_UpdateAddress_When_UpdateMethodIsCalled()
    {
        // Arrange
        var Address = new Address("58348000", "Rua Principal", "Riachão do Poço", "PB", "Paraíba");
        var novoPostalCode = "58348001";
        var novoStreet = "Rua Secundária";
        var novaCity = "Nova Cidade";
        var novaState = "SP";
        var novoStateName = "São Paulo";
        var novoNumber = "456";

        // Act
        Address.Update(novoStreet, novaCity, novaState, novoStateName, number: novoNumber);

        // Assert
        Address.PostalCode.Should().Be("58348000");
        Address.Street.Should().Be(novoStreet);
        Address.City.Should().Be(novaCity);
        Address.State.Should().Be("SP");
        Address.StateName.Should().Be(novoStateName);
        Address.Number.Should().Be(novoNumber);
        Address.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_AcceptNullOptionalFields_When_NotProvided()
    {
        // Arrange & Act
        var Address = new Address("58348000", "Rua Principal", "Riachão do Poço", "PB", "Paraíba");

        // Assert
        Address.Complement.Should().BeNull();
        Address.Number.Should().BeNull();
        Address.District.Should().BeNull();
        Address.IbgeCode.Should().BeNull();
        Address.AreaCode.Should().BeNull();
    }

    [Fact]
    public void Should_CreateCompleteAddress_When_AllFieldsProvided()
    {
        // Arrange & Act - Simular dados do ViaPostalCode
        var Address = new Address(
            postalCode: "58348000",
            street: "Rua Principal",
            city: "Riachão do Poço",
            state: "PB",
            stateName: "Paraíba",
            complement: null,
            number: null,
            district: "Centro",
            ibgeCode: "2512762",
            areaCode: "83"
        );

        // Assert
        Address.Should().NotBeNull();
        Address.PostalCode.Should().Be("58348000");
        Address.City.Should().Be("Riachão do Poço");
        Address.State.Should().Be("PB");
        Address.StateName.Should().Be("Paraíba");
        Address.IbgeCode.Should().Be("2512762");
        Address.AreaCode.Should().Be("83");
    }

    [Fact]
    public void Should_UpdateAddressCompletely_When_AllParametersProvided()
    {
        // Arrange
        var Address = new Address("58348000", "Rua Velha", "Riachão do Poço", "PB", "Paraíba");
        var initialUpdatedAt = Address.UpdatedAt;

        // Act
        Address.Update("Rua Nova", "Cidade Nova", "PE", "Pernambuco", "Apto 101", "789", "Bairro Novo", "1234567", "81");

        // Assert
        Address.Street.Should().Be("Rua Nova");
        Address.City.Should().Be("Cidade Nova");
        Address.State.Should().Be("PE");
        Address.StateName.Should().Be("Pernambuco");
        Address.Complement.Should().Be("Apto 101");
        Address.Number.Should().Be("789");
        Address.District.Should().Be("Bairro Novo");
        Address.IbgeCode.Should().Be("1234567");
        Address.AreaCode.Should().Be("81");
        Address.UpdatedAt.Should().NotBe(initialUpdatedAt);
        Address.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_UpdateAddressWithOptionalFields_When_CalledWithNulls()
    {
        // Arrange
        var Address = new Address("58348000", "Rua Principal", "Riachão do Poço", "PB", "Paraíba", "Casa", "123", "Centro");

        // Act
        Address.Update("Rua Nova", "Cidade Nova", "SP", "São Paulo");

        // Assert
        Address.Street.Should().Be("Rua Nova");
        Address.City.Should().Be("Cidade Nova");
        Address.State.Should().Be("SP");
        Address.StateName.Should().Be("São Paulo");
        Address.Complement.Should().BeNull();
        Address.Number.Should().BeNull();
        Address.District.Should().BeNull();
    }

    [Fact]
    public void Should_ConvertStateToUpperCaseOnUpdate_When_GivenLowerCase()
    {
        // Arrange
        var Address = new Address("58348000", "Rua Principal", "Riachão do Poço", "PB", "Paraíba");

        // Act
        Address.Update("Rua Nova", "Cidade Nova", "sp", "São Paulo");

        // Assert
        Address.State.Should().Be("SP");
    }

    [Fact]
    public void Should_HaveCreatedAtSet_When_AddressIsCreated()
    {
        // Arrange & Act
        var Address = new Address("58348000", "Rua Principal", "Riachão do Poço", "PB", "Paraíba");

        // Assert
        Address.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_NotHaveUpdatedAtSet_When_AddressIsJustCreated()
    {
        // Arrange & Act
        var Address = new Address("58348000", "Rua Principal", "Riachão do Poço", "PB", "Paraíba");

        // Assert
        Address.UpdatedAt.Should().BeNull();
    }
}
