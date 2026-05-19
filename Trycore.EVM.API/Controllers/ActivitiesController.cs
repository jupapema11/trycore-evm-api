using Microsoft.AspNetCore.Mvc;
using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;

namespace Trycore.EVM.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/activities")]
[Produces("application/json")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;

    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    /// <summary>Crea una actividad dentro del proyecto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(Guid projectId, CreateActivityDto dto)
    {
        try
        {
            var result = await _activityService.CreateAsync(projectId, dto);

            return CreatedAtAction(nameof(GetByProject), new { projectId }, result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Project {projectId} was not found." });
        }
    }

    /// <summary>Lista las actividades de un proyecto con indicadores EVM.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ActivityResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProject(Guid projectId)
    {
        var result = await _activityService.GetByProjectIdAsync(projectId);

        return Ok(result);
    }

    /// <summary>Actualiza una actividad existente.</summary>
    [HttpPut("{activityId:guid}")]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid projectId,
        Guid activityId,
        UpdateActivityDto dto)
    {
        var result = await _activityService.UpdateAsync(projectId, activityId, dto);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>Elimina una actividad.</summary>
    [HttpDelete("{activityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid projectId, Guid activityId)
    {
        var deleted = await _activityService.DeleteAsync(projectId, activityId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
