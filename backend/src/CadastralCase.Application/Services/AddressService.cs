using CadastralCase.Application.DTOs.Address;
using CadastralCase.Application.Validators.Address;
using CadastralCase.Domain.Entities;
using CadastralCase.Domain.Interfaces;
using FluentValidation;

namespace CadastralCase.Application.Services;

public class AddressService
{
    private readonly IAddressRepository _repository;
    private readonly IViaCepService _viaCepService;
    private readonly CreateAddressDtoValidator _createValidator;
    private readonly UpdateAddressDtoValidator _updateValidator;

    public AddressService(
        IAddressRepository repository,
        IViaCepService viaCepService)
    {
        _repository = repository;
        _viaCepService = viaCepService;
        _createValidator = new CreateAddressDtoValidator();
        _updateValidator = new UpdateAddressDtoValidator();
    }

    public async Task<AddressDto?> GetByIdAsync(Guid id)
    {
        var address = await _repository.GetByIdAsync(id);
        return address != null ? MapToDto(address) : null;
    }

    public async Task<IEnumerable<AddressDto>> GetAllAsync()
    {
        var addresses = await _repository.GetAllAsync();
        return addresses.Select(MapToDto);
    }

    public async Task<AddressDto> CreateAsync(CreateAddressDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Address address;

        if (dto.QueryViaCep)
        {
            var viaCepResponse = await _viaCepService.GetAddressAsync(dto.PostalCode);
            if (viaCepResponse == null)
                throw new InvalidOperationException("Postal code not found in ViaCEP");

            address = new Address(
                dto.PostalCode,
                viaCepResponse.Street ?? dto.Street ?? string.Empty,
                viaCepResponse.City ?? dto.City ?? string.Empty,
                viaCepResponse.State ?? dto.State ?? string.Empty,
                viaCepResponse.StateName ?? dto.StateName ?? string.Empty,
                dto.Complement,
                dto.Number,
                viaCepResponse.District ?? dto.District,
                viaCepResponse.IbgeCode ?? dto.IbgeCode,
                viaCepResponse.AreaCode ?? dto.AreaCode);
        }
        else
        {
            address = new Address(
                dto.PostalCode,
                dto.Street!,
                dto.City!,
                dto.State!,
                dto.StateName!,
                dto.Complement,
                dto.Number,
                dto.District,
                dto.IbgeCode,
                dto.AreaCode);
        }

        var created = await _repository.AddAsync(address);
        return MapToDto(created);
    }

    public async Task<AddressDto> UpdateAsync(Guid id, UpdateAddressDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var address = await _repository.GetByIdAsync(id);
        if (address == null)
            throw new InvalidOperationException("Address not found");

        address.Update(
            dto.Street,
            dto.City,
            dto.State,
            dto.StateName,
            dto.Complement,
            dto.Number,
            dto.District,
            dto.IbgeCode,
            dto.AreaCode);

        await _repository.UpdateAsync(address);
        return MapToDto(address);
    }

    public async Task DeleteAsync(Guid id)
    {
        var address = await _repository.GetByIdAsync(id);
        if (address == null)
            throw new InvalidOperationException("Address not found");

        await _repository.DeleteAsync(id);
    }

    public async Task<AddressDto?> GetByPostalCodeFromViaCepAsync(string postalCode)
    {
        var viaCepAddress = await _viaCepService.GetAddressAsync(postalCode);
        if (viaCepAddress == null)
            return null;

        return new AddressDto
        {
            PostalCode = viaCepAddress.PostalCode,
            Street = viaCepAddress.Street,
            Complement = viaCepAddress.Complement,
            District = viaCepAddress.District,
            City = viaCepAddress.City,
            State = viaCepAddress.State,
            StateName = viaCepAddress.StateName,
            IbgeCode = viaCepAddress.IbgeCode,
            AreaCode = viaCepAddress.AreaCode
        };
    }

    private static AddressDto MapToDto(Address address)
    {
        return new AddressDto
        {
            Id = address.Id,
            PostalCode = address.PostalCode,
            Street = address.Street,
            Complement = address.Complement,
            Number = address.Number,
            District = address.District,
            City = address.City,
            State = address.State,
            StateName = address.StateName,
            IbgeCode = address.IbgeCode,
            AreaCode = address.AreaCode,
            CreatedAt = address.CreatedAt,
            UpdatedAt = address.UpdatedAt
        };
    }
}
