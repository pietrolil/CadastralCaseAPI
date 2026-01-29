using CadastralCase.Application.DTOs.Address;
using FluentValidation;

namespace CadastralCase.Application.Validators.Address;

public class UpdateAddressDtoValidator : AbstractValidator<UpdateAddressDto>
{
    public UpdateAddressDtoValidator()
    {
        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required")
            .Must(BeValidPostalCode).WithMessage("Postal code must have 8 digits");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required")
            .Length(2).WithMessage("State must be 2 characters");

        RuleFor(x => x.StateName)
            .NotEmpty().WithMessage("State name is required");
    }

    private bool BeValidPostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
            return false;

        var cleanPostalCode = postalCode.Replace("-", "");
        return cleanPostalCode.Length == 8 && cleanPostalCode.All(char.IsDigit);
    }
}
