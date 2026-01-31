using CadastralCase.Application.DTOs.NaturalPerson;
using FluentValidation;

namespace CadastralCase.Application.Validators.NaturalPerson;

public class CreateNaturalPersonDtoValidator : AbstractValidator<CreateNaturalPersonDto>
{
    public CreateNaturalPersonDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.TaxId)
            .NotEmpty().WithMessage("Tax ID is required")
            .Must(BeValidCpf).WithMessage("Invalid Tax ID (CPF)");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required")
            .Must(BeInThePast).WithMessage("Birth date must be before current date")
            .Must(BeValidAge).WithMessage("Age must be between 0 and 150 years");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Phone must not exceed 20 characters");
    }

    private bool BeValidCpf(string taxId)
    {
        if (string.IsNullOrWhiteSpace(taxId))
            return false;

        var cpf = taxId.Replace(".", "").Replace("-", "");
        
        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        var multiplier1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        var remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        var digit = remainder.ToString();
        tempCpf += digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        digit += remainder.ToString();

        return cpf.EndsWith(digit);
    }

    private bool BeInThePast(DateTime birthDate)
    {
        return birthDate < DateTime.Now;
    }

    private bool BeValidAge(DateTime birthDate)
    {
        var age = DateTime.Now.Year - birthDate.Year;
        if (birthDate > DateTime.Now.AddYears(-age)) age--;
        return age >= 0 && age <= 150;
    }
}
