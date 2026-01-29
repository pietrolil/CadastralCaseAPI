using CadastralCase.Application.DTOs.Address;
using CadastralCase.Application.Validators.Address;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CadastralCase.Tests.Validators;

public class CreateAddressDtoValidatorTests
{
    private readonly CreateAddressDtoValidator _validator;

    public CreateAddressDtoValidatorTests()
    {
        _validator = new CreateAddressDtoValidator();
    }

    [Fact]
    public void Should_HaveError_When_PostalCodeIsEmpty()
    {
        // Arrange
        var dto = new CreateAddressDto { PostalCode = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode)
            .WithErrorMessage("Postal code is required");
    }

    [Fact]
    public void Should_HaveError_When_NumberIsEmpty()
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Number)
            .WithErrorMessage("Number is required");
    }

    [Theory]
    [InlineData("1234567")] // 7 dígitos
    [InlineData("123456789")] // 9 dígitos
    [InlineData("abcd5678")] // contém letras
    public void Should_HaveError_When_PostalCodeIsInvalid(string invalidPostalCode)
    {
        // Arrange
        var dto = new CreateAddressDto { PostalCode = invalidPostalCode };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode)
            .WithErrorMessage("Postal code must have 8 digits");
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("58348-000")] // com máscara
    public void Should_NotHaveError_When_PostalCodeIsValid(string validPostalCode)
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = validPostalCode,
            Number = "123",
            QueryViaCep = true // quando true, outros campos não são obrigatórios
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PostalCode);
    }

    [Fact]
    public void Should_HaveError_When_QueryViaCepIsFalseAndStreetIsEmpty()
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = "123",
            QueryViaCep = false,
            Street = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Street)
            .WithErrorMessage("Street is required when not querying ViaCEP");
    }

    [Fact]
    public void Should_HaveError_When_QueryViaCepIsFalseAndCityIsEmpty()
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = "123",
            QueryViaCep = false,
            City = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.City)
            .WithErrorMessage("City is required when not querying ViaCEP");
    }

    [Fact]
    public void Should_HaveError_When_QueryViaCepIsFalseAndStateIsEmpty()
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = "123",
            QueryViaCep = false,
            State = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State)
            .WithErrorMessage("State is required when not querying ViaCEP");
    }

    [Theory]
    [InlineData("P")] // 1 caractere
    [InlineData("PBA")] // 3 caracteres
    public void Should_HaveError_When_StateDoesNotHave2Characters(string invalidState)
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = "123",
            QueryViaCep = false,
            Street = "Rua Principal",
            City = "São Paulo",
            State = invalidState,
            StateName = "São Paulo"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State)
            .WithErrorMessage("State must be 2 characters");
    }

    [Fact]
    public void Should_HaveError_When_QueryViaCepIsFalseAndStateNameIsEmpty()
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = "123",
            QueryViaCep = false,
            StateName = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StateName)
            .WithErrorMessage("State name is required when not querying ViaCEP");
    }

    [Fact]
    public void Should_NotHaveError_When_QueryViaCepIsTrueAndOtherFieldsAreEmpty()
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = "123",
            QueryViaCep = true
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Street);
        result.ShouldNotHaveValidationErrorFor(x => x.City);
        result.ShouldNotHaveValidationErrorFor(x => x.State);
        result.ShouldNotHaveValidationErrorFor(x => x.StateName);
    }

    [Fact]
    public void Should_NotHaveError_When_AllRequiredFieldsAreProvidedAndQueryViaCepIsFalse()
    {
        // Arrange
        var dto = new CreateAddressDto 
        { 
            PostalCode = "12345678",
            Number = "123",
            QueryViaCep = false,
            Street = "Rua Principal",
            City = "São Paulo",
            State = "SP",
            StateName = "São Paulo"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
