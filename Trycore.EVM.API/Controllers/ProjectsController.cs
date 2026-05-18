using Microsoft.AspNetCore.Mvc;
using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;

namespace Trycore.EVM.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        var result = await _projectService.CreateAsync(dto);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _projectService.GetAllAsync();

        return Ok(result);
    }


    [HttpGet("{id}/summary")]
    public async Task<IActionResult> GetSummary(Guid id)
    {
        var result = await _projectService.GetSummaryAsync(id);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
