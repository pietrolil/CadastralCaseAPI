using CadastralCase.Application.DTOs.NaturalPerson;
using CadastralCase.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastralCase.API.Controllers;

[ApiController]
[Route("api/naturalPerson")]
[Produces("application/json")]
public class NaturalPersonController : ControllerBase
{
    private readonly NaturalPersonService _service;
    private readonly ILogger<NaturalPersonController> _logger;

    public NaturalPersonController(
        NaturalPersonService service,
        ILogger<NaturalPersonController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Returns all natural persons
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NaturalPersonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NaturalPersonDto>>> GetAll()
    {
        try
        {
            var persons = await _service.GetAllAsync();
            return Ok(persons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all natural persons");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Returns a natural person by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NaturalPersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NaturalPersonDto>> GetById(Guid id)
    {
        try
        {
            var pessoa = await _service.GetByIdAsync(id);
            
            if (pessoa == null)
                return NotFound(new { message = "Natural person not found" });

            return Ok(pessoa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching natural person by ID: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Creates a new natural person
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(NaturalPersonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NaturalPersonDto>> Create([FromBody] CreateNaturalPersonDto dto)
    {
        try
        {
            var pessoa = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating natural person");
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument error creating natural person");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating natural person");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Updates an existing natural person
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(NaturalPersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NaturalPersonDto>> Update(Guid id, [FromBody] UpdateNaturalPersonDto dto)
    {
        try
        {
            var pessoa = await _service.UpdateAsync(id, dto);
            return Ok(pessoa);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error updating natural person: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument error updating natural person: {Id}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating natural person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Deletes a natural person
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
            _logger.LogWarning(ex, "Natural person not found for deletion: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting natural person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Activates a natural person
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
            _logger.LogWarning(ex, "Natural person not found for activation: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating natural person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Deactivates a natural person
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
            _logger.LogWarning(ex, "Natural person not found for deactivation: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating natural person: {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
