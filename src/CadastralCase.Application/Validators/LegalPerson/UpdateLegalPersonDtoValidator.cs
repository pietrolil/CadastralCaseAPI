using CadastralCase.Application.DTOs.LegalPerson;
using FluentValidation;

namespace CadastralCase.Application.Validators.LegalPerson;

public class UpdateLegalPersonDtoValidator : AbstractValidator<UpdateLegalPersonDto>
{
    public UpdateLegalPersonDtoValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(300).WithMessage("Company name must not exceed 300 characters");

        RuleFor(x => x.TradeName)
            .NotEmpty().WithMessage("Trade name is required")
            .MaximumLength(200).WithMessage("Trade name must not exceed 200 characters");

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

    private bool BeInThePast(DateTime foundingDate)
    {
        return foundingDate < DateTime.Now;
    }

    private bool BeReasonableDate(DateTime foundingDate)
    {
        return foundingDate.Year > 1800;
    }
}
