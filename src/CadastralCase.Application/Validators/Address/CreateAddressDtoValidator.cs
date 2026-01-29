using CadastralCase.Application.DTOs.Address;
using FluentValidation;

namespace CadastralCase.Application.Validators.Address;

public class CreateAddressDtoValidator : AbstractValidator<CreateAddressDto>
{
    public CreateAddressDtoValidator()
    {
        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required")
            .Must(BeValidPostalCode).WithMessage("Postal code must have 8 digits");

        When(x => !x.QueryViaCep, () =>
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required when not querying ViaCEP");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required when not querying ViaCEP");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required when not querying ViaCEP")
                .Length(2).WithMessage("State must be 2 characters");

            RuleFor(x => x.StateName)
                .NotEmpty().WithMessage("State name is required when not querying ViaCEP");
        });
    }

    private bool BeValidPostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
            return false;

        var cleanPostalCode = postalCode.Replace("-", "");
        return cleanPostalCode.Length == 8 && cleanPostalCode.All(char.IsDigit);
    }
}
