using CadastralCase.Application.DTOs.Address;
using CadastralCase.Application.DTOs.LegalPerson;
using CadastralCase.Application.Validators.LegalPerson;
using CadastralCase.Domain.Entities;
using CadastralCase.Domain.Interfaces;
using FluentValidation;

namespace CadastralCase.Application.Services;

public class LegalPersonService
{
    private readonly ILegalPersonRepository _repository;
    private readonly IAddressRepository _addressRepository;
    private readonly CreateLegalPersonDtoValidator _createValidator;
    private readonly UpdateLegalPersonDtoValidator _updateValidator;

    public LegalPersonService(
        ILegalPersonRepository repository,
        IAddressRepository addressRepository)
    {
        _repository = repository;
        _addressRepository = addressRepository;
        _createValidator = new CreateLegalPersonDtoValidator();
        _updateValidator = new UpdateLegalPersonDtoValidator();
    }

    public async Task<LegalPersonDto?> GetByIdAsync(Guid id)
    {
        var company = await _repository.GetByIdAsync(id);
        if (company == null) return null;

        await LoadAddressAsync(company);
        return MapToDto(company);
    }

    public async Task<IEnumerable<LegalPersonDto>> GetAllAsync()
    {
        var companies = await _repository.GetAllAsync();
        var companiesList = companies.ToList();
        
        foreach (var company in companiesList)
        {
            await LoadAddressAsync(company);
        }
        
        return companiesList.Select(MapToDto);
    }

    public async Task<LegalPersonDto> CreateAsync(CreateLegalPersonDto dto)
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

        var company = new LegalPerson(
            dto.CompanyName,
            dto.TradeName,
            dto.TaxId,
            dto.FoundingDate,
            dto.Email,
            dto.Phone);

        if (dto.AddressId.HasValue)
            company.SetAddress(dto.AddressId.Value);

        var created = await _repository.AddAsync(company);
        return MapToDto(created);
    }

    public async Task<LegalPersonDto> UpdateAsync(Guid id, UpdateLegalPersonDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var company = await _repository.GetByIdAsync(id);
        if (company == null)
            throw new InvalidOperationException("Legal person not found");

        if (dto.AddressId.HasValue)
        {
            var address = await _addressRepository.GetByIdAsync(dto.AddressId.Value);
            if (address == null)
                throw new InvalidOperationException("Address not found");
        }

        company.Update(dto.CompanyName, dto.TradeName, dto.FoundingDate, dto.Email, dto.Phone);

        if (dto.AddressId.HasValue)
            company.SetAddress(dto.AddressId.Value);
        else
            company.RemoveAddress();

        await _repository.UpdateAsync(company);
        return MapToDto(company);
    }

    public async Task DeleteAsync(Guid id)
    {
        var company = await _repository.GetByIdAsync(id);
        if (company == null)
            throw new InvalidOperationException("Legal person not found");

        await _repository.DeleteAsync(id);
    }

    public async Task ActivateAsync(Guid id)
    {
        var company = await _repository.GetByIdAsync(id);
        if (company == null)
            throw new InvalidOperationException("Legal person not found");

        company.Activate();
        await _repository.UpdateAsync(company);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var company = await _repository.GetByIdAsync(id);
        if (company == null)
            throw new InvalidOperationException("Legal person not found");

        company.Deactivate();
        await _repository.UpdateAsync(company);
    }

    private static LegalPersonDto MapToDto(LegalPerson company)
    {
        return new LegalPersonDto
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            TradeName = company.TradeName,
            TaxId = company.TaxId,
            Email = company.Email,
            Phone = company.Phone,
            FoundingDate = company.FoundingDate,
            AddressId = company.AddressId,
            Address = company.Address != null ? MapAddressToDto(company.Address) : null,
            CreatedAt = company.CreatedAt,
            UpdatedAt = company.UpdatedAt,
            IsActive = company.IsActive
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

    private async Task LoadAddressAsync(LegalPerson company)
    {
        if (company.AddressId.HasValue)
        {
            var address = await _addressRepository.GetByIdAsync(company.AddressId.Value);
            if (address != null)
            {
                company.SetAddress(company.AddressId.Value);
                // Use reflection to set the Address property since it's private set
                var addressProperty = typeof(LegalPerson).GetProperty("Address");
                addressProperty?.SetValue(company, address);
            }
        }
    }
}
