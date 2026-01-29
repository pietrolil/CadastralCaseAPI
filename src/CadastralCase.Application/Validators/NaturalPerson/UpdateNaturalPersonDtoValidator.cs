using CadastralCase.Application.DTOs.NaturalPerson;
using FluentValidation;

namespace CadastralCase.Application.Validators.NaturalPerson;

public class UpdateNaturalPersonDtoValidator : AbstractValidator<UpdateNaturalPersonDto>
{
    public UpdateNaturalPersonDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

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
