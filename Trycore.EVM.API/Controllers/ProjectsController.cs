using Microsoft.AspNetCore.Mvc;
using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;

namespace Trycore.EVM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>Crea un nuevo proyecto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        var result = await _projectService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    /// <summary>Lista todos los proyectos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _projectService.GetAllAsync();

        return Ok(result);
    }

    /// <summary>Actualiza un proyecto existente.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProjectResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, UpdateProjectDto dto)
    {
        var result = await _projectService.UpdateAsync(id, dto);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>Elimina un proyecto y sus actividades.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _projectService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    /// <summary>Obtiene el resumen consolidado EVM del proyecto.</summary>
    [HttpGet("{id:guid}/summary")]
    [ProducesResponseType(typeof(ProjectSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummary(Guid id)
    {
        var result = await _projectService.GetSummaryAsync(id);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
