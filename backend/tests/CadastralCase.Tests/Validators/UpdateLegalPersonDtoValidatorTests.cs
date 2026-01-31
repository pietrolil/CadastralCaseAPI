using CadastralCase.Application.DTOs.LegalPerson;
using CadastralCase.Application.Validators.LegalPerson;
using FluentValidation.TestHelper;
using Xunit;

namespace CadastralCase.Tests.Validators;

public class UpdateLegalPersonDtoValidatorTests
{
    private readonly UpdateLegalPersonDtoValidator _validator;

    public UpdateLegalPersonDtoValidatorTests()
    {
        _validator = new UpdateLegalPersonDtoValidator();
    }

    [Fact]
    public void Should_HaveError_When_CompanyNameIsEmpty()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto { CompanyName = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
            .WithErrorMessage("Company name is required");
    }

    [Fact]
    public void Should_HaveError_When_CompanyNameExceeds300Characters()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto { CompanyName = new string('A', 301) };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
            .WithErrorMessage("Company name must not exceed 300 characters");
    }

    [Fact]
    public void Should_HaveError_When_TradeNameIsEmpty()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto { TradeName = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TradeName)
            .WithErrorMessage("Trade name is required");
    }

    [Fact]
    public void Should_HaveError_When_TradeNameExceeds200Characters()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto { TradeName = new string('A', 201) };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TradeName)
            .WithErrorMessage("Trade name must not exceed 200 characters");
    }

    [Fact]
    public void Should_HaveError_When_FoundingDateIsInFuture()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto 
        { 
            CompanyName = "Empresa XYZ LTDA",
            TradeName = "XYZ Tech",
            FoundingDate = DateTime.Now.AddDays(1)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FoundingDate)
            .WithErrorMessage("Founding date must be before current date");
    }

    [Fact]
    public void Should_HaveError_When_FoundingDateIsBefore1800()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto 
        { 
            CompanyName = "Empresa XYZ LTDA",
            TradeName = "XYZ Tech",
            FoundingDate = new DateTime(1799, 12, 31)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FoundingDate)
            .WithErrorMessage("Founding date must be after 1800");
    }

    [Fact]
    public void Should_HaveError_When_EmailIsInvalid()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto 
        { 
            CompanyName = "Empresa XYZ LTDA",
            TradeName = "XYZ Tech",
            FoundingDate = DateTime.Now.AddYears(-5),
            Email = "email-invalido"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format");
    }

    [Fact]
    public void Should_NotHaveError_When_EmailIsValid()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto 
        { 
            CompanyName = "Empresa XYZ LTDA",
            TradeName = "XYZ Tech",
            FoundingDate = DateTime.Now.AddYears(-5),
            Email = "contato@xyz.com"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_HaveError_When_PhoneExceeds20Characters()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto 
        { 
            CompanyName = "Empresa XYZ LTDA",
            TradeName = "XYZ Tech",
            FoundingDate = DateTime.Now.AddYears(-5),
            Phone = new string('1', 21)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone)
            .WithErrorMessage("Phone must not exceed 20 characters");
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto 
        { 
            CompanyName = "Empresa XYZ LTDA",
            TradeName = "XYZ Tech",
            FoundingDate = DateTime.Now.AddYears(-5),
            Email = "contato@xyz.com",
            Phone = "11999999999"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_NotHaveError_When_OptionalFieldsAreEmpty()
    {
        // Arrange
        var dto = new UpdateLegalPersonDto 
        { 
            CompanyName = "Empresa XYZ LTDA",
            TradeName = "XYZ Tech",
            FoundingDate = DateTime.Now.AddYears(-5)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
