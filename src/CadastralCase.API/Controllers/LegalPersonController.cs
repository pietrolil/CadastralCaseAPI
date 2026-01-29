using CadastralCase.Application.DTOs.LegalPerson;
using CadastralCase.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastralCase.API.Controllers;

[ApiController]
[Route("api/legalPerson")]
[Produces("application/json")]
public class LegalPersonController : ControllerBase
{
    private readonly LegalPersonService _service;
    private readonly ILogger<LegalPersonController> _logger;

    public LegalPersonController(
        LegalPersonService service,
        ILogger<LegalPersonController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Returns all legal persons
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LegalPersonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LegalPersonDto>>> GetAll()
    {
        try
        {
            var companies = await _service.GetAllAsync();
            return Ok(companies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all legal persons");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Returns a legal person by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LegalPersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LegalPersonDto>> GetById(Guid id)
    {
        try
        {
            var pessoa = await _service.GetByIdAsync(id);
            
            if (pessoa == null)
                return NotFound(new { message = "Legal person not found" });

            return Ok(pessoa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching legal person by ID: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Creates a new legal person
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LegalPersonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LegalPersonDto>> Create([FromBody] CreateLegalPersonDto dto)
    {
        try
        {
            var pessoa = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating legal person");
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument error creating legal person");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating legal person");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Updates an existing legal person
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(LegalPersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LegalPersonDto>> Update(Guid id, [FromBody] UpdateLegalPersonDto dto)
    {
        try
        {
            var pessoa = await _service.UpdateAsync(id, dto);
            return Ok(pessoa);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error updating legal person: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument error updating legal person: {Id}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating legal person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Deletes a legal person
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
            _logger.LogWarning(ex, "Legal person not found for deletion: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting legal person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Activates a legal person
    /// </summary>
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _service.ActivateAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Legal person not found for activation: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating legal person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Deactivates a legal person
    /// </summary>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _service.DeactivateAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Legal person not found for deactivation: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating legal person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
