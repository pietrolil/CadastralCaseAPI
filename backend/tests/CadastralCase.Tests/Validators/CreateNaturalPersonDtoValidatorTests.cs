using CadastralCase.Application.DTOs.NaturalPerson;
using CadastralCase.Application.Validators.NaturalPerson;
using FluentValidation.TestHelper;
using Xunit;

namespace CadastralCase.Tests.Validators;

public class CreateNaturalPersonDtoValidatorTests
{
    private readonly CreateNaturalPersonDtoValidator _validator;

    public CreateNaturalPersonDtoValidatorTests()
    {
        _validator = new CreateNaturalPersonDtoValidator();
    }

    [Fact]
    public void Should_HaveError_When_NameIsEmpty()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto { Name = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required");
    }

    [Fact]
    public void Should_HaveError_When_NameExceeds200Characters()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto { Name = new string('A', 201) };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must not exceed 200 characters");
    }

    [Fact]
    public void Should_HaveError_When_TaxIdIsEmpty()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto { TaxId = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TaxId)
            .WithErrorMessage("Tax ID is required");
    }

    [Theory]
    [InlineData("12345678901")] // CPF inválido
    [InlineData("00000000000")] // todos iguais
    [InlineData("123")] // muito curto
    public void Should_HaveError_When_TaxIdIsInvalid(string invalidTaxId)
    {
        // Arrange
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = invalidTaxId,
            BirthDate = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TaxId)
            .WithErrorMessage("Invalid Tax ID (CPF)");
    }

    [Theory]
    [InlineData("52998224725")] // CPF válido
    [InlineData("529.982.247-25")] // CPF válido com máscara
    public void Should_NotHaveError_When_TaxIdIsValid(string validTaxId)
    {
        // Arrange
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = validTaxId,
            BirthDate = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Fact]
    public void Should_HaveError_When_BirthDateIsEmpty()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = default
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorMessage("Birth date is required");
    }

    [Fact]
    public void Should_HaveError_When_BirthDateIsInFuture()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = DateTime.Now.AddDays(1)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorMessage("Birth date must be before current date");
    }

    [Fact]
    public void Should_HaveError_When_AgeIsOver150Years()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = DateTime.Now.AddYears(-151)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorMessage("Age must be between 0 and 150 years");
    }

    [Fact]
    public void Should_HaveError_When_EmailIsInvalid()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = DateTime.Now.AddYears(-25),
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
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = DateTime.Now.AddYears(-25),
            Email = "joao@email.com"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_NotHaveError_When_EmailIsEmpty()
    {
        // Arrange
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = DateTime.Now.AddYears(-25),
            Email = ""
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
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = DateTime.Now.AddYears(-25),
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
        var dto = new CreateNaturalPersonDto 
        { 
            Name = "João Silva",
            TaxId = "52998224725",
            BirthDate = DateTime.Now.AddYears(-25),
            Email = "joao@email.com",
            Phone = "11999999999"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
