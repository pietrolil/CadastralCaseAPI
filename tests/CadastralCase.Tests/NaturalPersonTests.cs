using CadastralCase.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CadastralCase.Tests.Domain;

public class NaturalPersonTests
{
    [Fact]
    public void Should_CreateNaturalPerson_When_GivenValidData()
    {
        // Arrange
        var Name = "João da Silva";
        var TaxId = "52998224725";
        var BirthDate = DateTime.Now.AddYears(-25);
        var email = "joao@email.com";
        var Phone = "(11) 98765-4321";

        // Act
        var pessoa = new NaturalPerson(Name, TaxId, BirthDate, email, Phone);

        // Assert
        pessoa.Should().NotBeNull();
        pessoa.Name.Should().Be(Name);
        pessoa.TaxId.Should().Be(TaxId);
        pessoa.BirthDate.Should().Be(BirthDate);
        pessoa.Email.Should().Be(email);
        pessoa.Phone.Should().Be(Phone);
        pessoa.IsActive.Should().BeTrue();
        pessoa.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_AcceptTaxId_When_GivenWithMask()
    {
        // Arrange
        var Name = "João da Silva";
        var TaxIdComMascara = "529.982.247-25";
        var BirthDate = DateTime.Now.AddYears(-25);

        // Act
        var pessoa = new NaturalPerson(Name, TaxIdComMascara, BirthDate);

        // Assert
        pessoa.TaxId.Should().Be("52998224725");
    }

    [Fact]
    public void Should_UpdateNaturalPersonData_When_UpdateMethodIsCalled()
    {
        // Arrange
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));
        var novoName = "João da Silva Santos";
        var novoEmail = "joao.santos@email.com";
        var novaData = DateTime.Now.AddYears(-30);

        // Act
        pessoa.Update(novoName, novaData, novoEmail, null);

        // Assert
        pessoa.Name.Should().Be(novoName);
        pessoa.Email.Should().Be(novoEmail);
        pessoa.BirthDate.Should().Be(novaData);
        pessoa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_AssociateAddress_When_SetAddressIsCalled()
    {
        // Arrange
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));
        var AddressId = Guid.NewGuid();

        // Act
        pessoa.SetAddress(AddressId);

        // Assert
        pessoa.AddressId.Should().Be(AddressId);
        pessoa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_RemoveAddress_When_RemoveAddressIsCalled()
    {
        // Arrange
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));
        pessoa.SetAddress(Guid.NewGuid());

        // Act
        pessoa.RemoveAddress();

        // Assert
        pessoa.AddressId.Should().BeNull();
        pessoa.Address.Should().BeNull();
    }

    [Fact]
    public void Should_ActivateAndDeactivateNaturalPerson_When_MethodsAreCalled()
    {
        // Arrange
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));

        // Act & Assert
        pessoa.IsActive.Should().BeTrue();

        // Desativar
        pessoa.Deactivate();
        pessoa.IsActive.Should().BeFalse();

        // Reativar
        pessoa.Activate();
        pessoa.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("52998224725")]
    [InlineData("11144477735")]
    public void Should_AcceptTaxId_When_GivenValidTaxId(string TaxIdValido)
    {
        // Arrange
        var Name = "João da Silva";
        var BirthDate = DateTime.Now.AddYears(-25);

        // Act
        var pessoa = new NaturalPerson(Name, TaxIdValido, BirthDate);

        // Assert
        pessoa.Should().NotBeNull();
        pessoa.TaxId.Should().Be(TaxIdValido);
    }

    [Fact]
    public void Should_UpdateOnlyRequiredFields_When_OptionalFieldsAreNull()
    {
        // Arrange
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25), "joao@email.com", "11999999999");
        var newName = "João Silva";
        var newBirthDate = DateTime.Now.AddYears(-30);

        // Act
        pessoa.Update(newName, newBirthDate);

        // Assert
        pessoa.Name.Should().Be(newName);
        pessoa.BirthDate.Should().Be(newBirthDate);
        pessoa.Email.Should().BeNull();
        pessoa.Phone.Should().BeNull();
        pessoa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_CreateWithOptionalFields_When_EmailAndPhoneProvided()
    {
        // Arrange
        var Name = "João da Silva";
        var TaxId = "52998224725";
        var BirthDate = DateTime.Now.AddYears(-25);
        var email = "joao@email.com";
        var Phone = "11999999999";

        // Act
        var pessoa = new NaturalPerson(Name, TaxId, BirthDate, email, Phone);

        // Assert
        pessoa.Email.Should().Be(email);
        pessoa.Phone.Should().Be(Phone);
    }

    [Fact]
    public void Should_CreateWithNullOptionalFields_When_NotProvided()
    {
        // Arrange & Act
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));

        // Assert
        pessoa.Email.Should().BeNull();
        pessoa.Phone.Should().BeNull();
    }

    [Fact]
    public void Should_HaveCreatedAtSet_When_NaturalPersonIsCreated()
    {
        // Arrange & Act
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));

        // Assert
        pessoa.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_NotHaveUpdatedAtSet_When_NaturalPersonIsJustCreated()
    {
        // Arrange & Act
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));

        // Assert
        pessoa.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Should_HaveValidId_When_NaturalPersonIsCreated()
    {
        // Arrange & Act
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));

        // Assert
        pessoa.Id.Should().NotBeEmpty();
        pessoa.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Should_SetUpdatedAt_When_SetAddressIsCalled()
    {
        // Arrange
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));
        var initialUpdatedAt = pessoa.UpdatedAt;

        // Act
        pessoa.SetAddress(Guid.NewGuid());

        // Assert
        pessoa.UpdatedAt.Should().NotBe(initialUpdatedAt);
        pessoa.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_SetUpdatedAt_When_RemoveAddressIsCalled()
    {
        // Arrange
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));
        pessoa.SetAddress(Guid.NewGuid());
        var initialUpdatedAt = pessoa.UpdatedAt;

        // Act
        System.Threading.Thread.Sleep(10);
        pessoa.RemoveAddress();

        // Assert
        pessoa.UpdatedAt.Should().NotBe(initialUpdatedAt);
    }

    [Fact]
    public void Should_BeActiveByDefault_When_NaturalPersonIsCreated()
    {
        // Arrange & Act
        var pessoa = new NaturalPerson("João", "52998224725", DateTime.Now.AddYears(-25));

        // Assert
        pessoa.IsActive.Should().BeTrue();
    }
}
