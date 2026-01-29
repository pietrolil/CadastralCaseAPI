using CadastralCase.Application.DTOs.LegalPerson;
using FluentValidation;

namespace CadastralCase.Application.Validators.LegalPerson;

public class CreateLegalPersonDtoValidator : AbstractValidator<CreateLegalPersonDto>
{
    public CreateLegalPersonDtoValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(300).WithMessage("Company name must not exceed 300 characters");

        RuleFor(x => x.TradeName)
            .NotEmpty().WithMessage("Trade name is required")
            .MaximumLength(200).WithMessage("Trade name must not exceed 200 characters");

        RuleFor(x => x.TaxId)
            .NotEmpty().WithMessage("Tax ID is required")
            .Must(BeValidCnpj).WithMessage("Invalid Tax ID (CNPJ)");

        RuleFor(x => x.FoundingDate)
            .NotEmpty().WithMessage("Founding date is required")
            .Must(BeInThePast).WithMessage("Founding date must be before current date")
            .Must(BeReasonableDate).WithMessage("Founding date must be after 1800");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Phone must not exceed 20 characters");
    }

    private bool BeValidCnpj(string taxId)
    {
        if (string.IsNullOrWhiteSpace(taxId))
            return false;

        var cnpj = taxId.Replace(".", "").Replace("-", "").Replace("/", "");
        
        if (cnpj.Length != 14)
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        var multiplier1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCnpj = cnpj.Substring(0, 12);
        var sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

        var remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        var digit = remainder.ToString();
        tempCnpj += digit;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        digit += remainder.ToString();

        return cnpj.EndsWith(digit);
    }

    private bool BeInThePast(DateTime foundingDate)
    {
        return foundingDate < DateTime.Now;
    }

    private bool BeReasonableDate(DateTime foundingDate)
    {
        return foundingDate.Year > 1800;
    }
}
