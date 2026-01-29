using CadastralCase.Application.DTOs.Address;
using CadastralCase.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastralCase.API.Controllers;

[ApiController]
[Route("api/address")]
[Produces("application/json")]
public class AddressController : ControllerBase
{
    private readonly AddressService _service;
    private readonly ILogger<AddressController> _logger;

    public AddressController(
        AddressService service,
        ILogger<AddressController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Returns all addresses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AddressDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAll()
    {
        try
        {
            var addresses = await _service.GetAllAsync();
            return Ok(addresses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all addresses");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Returns an address by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressDto>> GetById(Guid id)
    {
        try
        {
            var endereco = await _service.GetByIdAsync(id);
            
            if (endereco == null)
                return NotFound(new { message = "Address not found" });

            return Ok(endereco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching address by ID: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Creates a new address (can automatically query ViaCEP)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AddressDto>> Create([FromBody] CreateAddressDto dto)
    {
        try
        {
            var endereco = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = endereco.Id }, endereco);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating address");
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument error creating address");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating address");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Updates an existing address
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressDto>> Update(Guid id, [FromBody] UpdateAddressDto dto)
    {
        try
        {
            var endereco = await _service.UpdateAsync(id, dto);
            return Ok(endereco);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error updating address: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument error updating address: {Id}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating address: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Deletes an address
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Address not found for deletion: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting address: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
