using CadastralCase.Application.DTOs.Address;
using CadastralCase.Application.Validators.Address;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CadastralCase.Tests.Validators;

public class UpdateAddressDtoValidatorTests
{
    private readonly UpdateAddressDtoValidator _validator;

    public UpdateAddressDtoValidatorTests()
    {
        _validator = new UpdateAddressDtoValidator();
    }

    [Fact]
    public void Should_HaveError_When_PostalCodeIsEmpty()
    {
        // Arrange
        var dto = new UpdateAddressDto { PostalCode = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }

    [Fact]
    public void Should_HaveError_When_StreetIsEmpty()
    {
        // Arrange
        var dto = new UpdateAddressDto { Street = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Street);
    }

    [Fact]
    public void Should_HaveError_When_NumberIsEmpty()
    {
        // Arrange
        var dto = new UpdateAddressDto { Number = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Number);
    }

    [Fact]
    public void Should_HaveError_When_CityIsEmpty()
    {
        // Arrange
        var dto = new UpdateAddressDto { City = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void Should_HaveError_When_StateIsEmpty()
    {
        // Arrange
        var dto = new UpdateAddressDto { State = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Theory]
    [InlineData("P")]
    [InlineData("PBA")]
    public void Should_HaveError_When_StateDoesNotHave2Characters(string invalidState)
    {
        // Arrange
        var dto = new UpdateAddressDto 
        { 
            PostalCode = "12345678",
            Street = "Rua Principal",
            Number = "123",
            City = "São Paulo",
            State = invalidState,
            StateName = "São Paulo"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void Should_HaveError_When_StateNameIsEmpty()
    {
        // Arrange
        var dto = new UpdateAddressDto { StateName = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StateName);
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        // Arrange
        var dto = new UpdateAddressDto 
        { 
            PostalCode = "12345678",
            Street = "Rua Principal",
            Number = "123",
            City = "São Paulo",
            State = "SP",
            StateName = "São Paulo"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_NotHaveError_When_PostalCodeHasMask()
    {
        // Arrange
        var dto = new UpdateAddressDto 
        { 
            PostalCode = "12345-678",
            Street = "Rua Principal",
            Number = "123",
            City = "São Paulo",
            State = "SP",
            StateName = "São Paulo"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PostalCode);
    }
}
