using Microsoft.AspNetCore.Mvc;
using Trycore.EVM.Application.Interfaces;

namespace Trycore.EVM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvmController : ControllerBase
{
    private readonly IEvmCalculationService _evmService;

    public EvmController(IEvmCalculationService evmService)
    {
        _evmService = evmService;
    }

    [HttpGet("pv")]
    public IActionResult CalculatePV(decimal plannedPercent, decimal bac)
    {
        var result = _evmService.CalculatePV(plannedPercent, bac);

        return Ok(result);
    }

    [HttpGet("ev")]
    public IActionResult CalculateEV(decimal actualPercent, decimal bac)
    {
        var result = _evmService.CalculateEV(actualPercent, bac);

        return Ok(result);
    }
}