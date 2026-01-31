using CadastralCase.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CadastralCase.Tests.Domain;

public class LegalPersonTests
{
    [Fact]
    public void Should_CreateLegalPerson_When_GivenValidData()
    {
        // Arrange
        var CompanyName = "Empresa XYZ LTDA";
        var TradeName = "XYZ Tech";
        var TaxId = "11222333000181";
        var FoundingDate = DateTime.Now.AddYears(-5);
        var email = "contato@xyz.com";
        var Phone = "(11) 3333-4444";

        // Act
        var empresa = new LegalPerson(CompanyName, TradeName, TaxId, FoundingDate, email, Phone);

        // Assert
        empresa.Should().NotBeNull();
        empresa.CompanyName.Should().Be(CompanyName);
        empresa.TradeName.Should().Be(TradeName);
        empresa.TaxId.Should().Be(TaxId);
        empresa.FoundingDate.Should().Be(FoundingDate);
        empresa.Email.Should().Be(email);
        empresa.Phone.Should().Be(Phone);
        empresa.IsActive.Should().BeTrue();
        empresa.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_AcceptTaxId_When_GivenWithMask()
    {
        // Arrange
        var CompanyName = "Empresa XYZ LTDA";
        var TradeName = "XYZ Tech";
        var TaxIdComMascara = "11.222.333/0001-81"; // TaxId válido com máscara
        var FoundingDate = DateTime.Now.AddYears(-5);

        // Act
        var empresa = new LegalPerson(CompanyName, TradeName, TaxIdComMascara, FoundingDate);

        // Assert
        empresa.TaxId.Should().Be("11222333000181"); // Deve armazenar sem máscara
    }

    [Fact]
    public void Should_UpdateLegalPersonData_When_UpdateMethodIsCalled()
    {
        // Arrange
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));
        var novaCompanyName = "Empresa XYZ LTDA ME";
        var novoTradeName = "XYZ Technology";
        var novoEmail = "contato@xyztech.com";
        var novaData = DateTime.Now.AddYears(-10);

        // Act
        empresa.Update(novaCompanyName, novoTradeName, novaData, novoEmail, null);

        // Assert
        empresa.CompanyName.Should().Be(novaCompanyName);
        empresa.TradeName.Should().Be(novoTradeName);
        empresa.Email.Should().Be(novoEmail);
        empresa.FoundingDate.Should().Be(novaData);
        empresa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_AssociateAddress_When_SetAddressIsCalled()
    {
        // Arrange
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));
        var AddressId = Guid.NewGuid();

        // Act
        empresa.SetAddress(AddressId);

        // Assert
        empresa.AddressId.Should().Be(AddressId);
        empresa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_RemoveAddress_When_RemoveAddressIsCalled()
    {
        // Arrange
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));
        empresa.SetAddress(Guid.NewGuid());

        // Act
        empresa.RemoveAddress();

        // Assert
        empresa.AddressId.Should().BeNull();
        empresa.Address.Should().BeNull();
    }

    [Fact]
    public void Should_ActivateAndDeactivateLegalPerson_When_MethodsAreCalled()
    {
        // Arrange
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));

        // Act & Assert - Ativa por padrão
        empresa.IsActive.Should().BeTrue();

        // Desativar
        empresa.Deactivate();
        empresa.IsActive.Should().BeFalse();

        // Reativar
        empresa.Activate();
        empresa.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("11222333000181")] // TaxId válido
    [InlineData("11444777000161")] // TaxId válido
    public void Should_AcceptTaxId_When_GivenValidTaxId(string TaxIdValido)
    {
        // Arrange
        var CompanyName = "Empresa XYZ LTDA";
        var TradeName = "XYZ Tech";
        var FoundingDate = DateTime.Now.AddYears(-5);

        // Act
        var empresa = new LegalPerson(CompanyName, TradeName, TaxIdValido, FoundingDate);

        // Assert
        empresa.Should().NotBeNull();
        empresa.TaxId.Should().Be(TaxIdValido);
    }

    [Fact]
    public void Should_UpdateOnlyRequiredFields_When_OptionalFieldsAreNull()
    {
        // Arrange
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5), "contato@xyz.com", "11999999999");
        var newCompanyName = "Empresa ABC LTDA";
        var newTradeName = "ABC Tech";
        var newFoundingDate = DateTime.Now.AddYears(-10);

        // Act
        empresa.Update(newCompanyName, newTradeName, newFoundingDate);

        // Assert
        empresa.CompanyName.Should().Be(newCompanyName);
        empresa.TradeName.Should().Be(newTradeName);
        empresa.FoundingDate.Should().Be(newFoundingDate);
        empresa.Email.Should().BeNull();
        empresa.Phone.Should().BeNull();
        empresa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_CreateWithOptionalFields_When_EmailAndPhoneProvided()
    {
        // Arrange
        var CompanyName = "Empresa XYZ LTDA";
        var TradeName = "XYZ Tech";
        var TaxId = "11222333000181";
        var FoundingDate = DateTime.Now.AddYears(-5);
        var email = "contato@xyz.com";
        var Phone = "11999999999";

        // Act
        var empresa = new LegalPerson(CompanyName, TradeName, TaxId, FoundingDate, email, Phone);

        // Assert
        empresa.Email.Should().Be(email);
        empresa.Phone.Should().Be(Phone);
    }

    [Fact]
    public void Should_CreateWithNullOptionalFields_When_NotProvided()
    {
        // Arrange & Act
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));

        // Assert
        empresa.Email.Should().BeNull();
        empresa.Phone.Should().BeNull();
    }

    [Fact]
    public void Should_HaveCreatedAtSet_When_LegalPersonIsCreated()
    {
        // Arrange & Act
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));

        // Assert
        empresa.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_NotHaveUpdatedAtSet_When_LegalPersonIsJustCreated()
    {
        // Arrange & Act
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));

        // Assert
        empresa.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Should_HaveValidId_When_LegalPersonIsCreated()
    {
        // Arrange & Act
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));

        // Assert
        empresa.Id.Should().NotBeEmpty();
        empresa.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Should_SetUpdatedAt_When_SetAddressIsCalled()
    {
        // Arrange
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));
        var initialUpdatedAt = empresa.UpdatedAt;

        // Act
        empresa.SetAddress(Guid.NewGuid());

        // Assert
        empresa.UpdatedAt.Should().NotBe(initialUpdatedAt);
        empresa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_SetUpdatedAt_When_RemoveAddressIsCalled()
    {
        // Arrange
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));
        empresa.SetAddress(Guid.NewGuid());
        var initialUpdatedAt = empresa.UpdatedAt;

        // Act
        System.Threading.Thread.Sleep(10);
        empresa.RemoveAddress();

        // Assert
        empresa.UpdatedAt.Should().NotBe(initialUpdatedAt);
    }

    [Fact]
    public void Should_BeActiveByDefault_When_LegalPersonIsCreated()
    {
        // Arrange & Act
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", "11222333000181", DateTime.Now.AddYears(-5));

        // Assert
        empresa.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Should_RemoveTaxIdMask_When_GivenWithSlash()
    {
        // Arrange
        var TaxIdComMascara = "11.222.333/0001-81";

        // Act
        var empresa = new LegalPerson("Empresa XYZ", "XYZ", TaxIdComMascara, DateTime.Now.AddYears(-5));

        // Assert
        empresa.TaxId.Should().Be("11222333000181");
        empresa.TaxId.Should().NotContain(".");
        empresa.TaxId.Should().NotContain("-");
        empresa.TaxId.Should().NotContain("/");
    }
}
