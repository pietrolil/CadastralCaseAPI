using CadastralCase.Application.DTOs.Address;
using CadastralCase.Application.DTOs.NaturalPerson;
using CadastralCase.Application.Validators.NaturalPerson;
using CadastralCase.Domain.Entities;
using CadastralCase.Domain.Interfaces;
using FluentValidation;

namespace CadastralCase.Application.Services;

public class NaturalPersonService
{
    private readonly INaturalPersonRepository _repository;
    private readonly IAddressRepository _addressRepository;
    private readonly CreateNaturalPersonDtoValidator _createValidator;
    private readonly UpdateNaturalPersonDtoValidator _updateValidator;

    public NaturalPersonService(
        INaturalPersonRepository repository,
        IAddressRepository addressRepository)
    {
        _repository = repository;
        _addressRepository = addressRepository;
        _createValidator = new CreateNaturalPersonDtoValidator();
        _updateValidator = new UpdateNaturalPersonDtoValidator();
    }

    public async Task<NaturalPersonDto?> GetByIdAsync(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);
        return person != null ? MapToDto(person) : null;
    }

    public async Task<IEnumerable<NaturalPersonDto>> GetAllAsync()
    {
        var persons = await _repository.GetAllAsync();
        return persons.Select(MapToDto);
    }

    public async Task<NaturalPersonDto> CreateAsync(CreateNaturalPersonDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await _repository.GetByTaxIdAsync(dto.TaxId);
        if (existing != null)
            throw new InvalidOperationException($"TaxId {dto.TaxId} is already registered");

        if (dto.AddressId.HasValue)
        {
            var address = await _addressRepository.GetByIdAsync(dto.AddressId.Value);
            if (address == null)
                throw new InvalidOperationException("Address not found");
        }

        var person = new NaturalPerson(
            dto.Name,
            dto.TaxId,
            dto.BirthDate,
            dto.Email,
            dto.Phone);

        if (dto.AddressId.HasValue)
            person.SetAddress(dto.AddressId.Value);

        var created = await _repository.AddAsync(person);
        return MapToDto(created);
    }

    public async Task<NaturalPersonDto> UpdateAsync(Guid id, UpdateNaturalPersonDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var person = await _repository.GetByIdAsync(id);
        if (person == null)
            throw new InvalidOperationException("Natural person not found");

        if (dto.AddressId.HasValue)
        {
            var address = await _addressRepository.GetByIdAsync(dto.AddressId.Value);
            if (address == null)
                throw new InvalidOperationException("Address not found");
        }

        person.Update(dto.Name, dto.BirthDate, dto.Email, dto.Phone);

        if (dto.AddressId.HasValue)
            person.SetAddress(dto.AddressId.Value);
        else
            person.RemoveAddress();

        await _repository.UpdateAsync(person);
        return MapToDto(person);
    }

    public async Task DeleteAsync(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);
        if (person == null)
            throw new InvalidOperationException("Natural person not found");

        await _repository.DeleteAsync(id);
    }

    public async Task ActivateAsync(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);
        if (person == null)
            throw new InvalidOperationException("Natural person not found");

        person.Activate();
        await _repository.UpdateAsync(person);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);
        if (person == null)
            throw new InvalidOperationException("Natural person not found");

        person.Deactivate();
        await _repository.UpdateAsync(person);
    }

    private static NaturalPersonDto MapToDto(NaturalPerson person)
    {
        return new NaturalPersonDto
        {
            Id = person.Id,
            Name = person.Name,
            TaxId = person.TaxId,
            Email = person.Email,
            Phone = person.Phone,
            BirthDate = person.BirthDate,
            AddressId = person.AddressId,
            Address = person.Address != null ? MapAddressToDto(person.Address) : null,
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt,
            IsActive = person.IsActive
        };
    }

    private static AddressDto MapAddressToDto(Address address)
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
