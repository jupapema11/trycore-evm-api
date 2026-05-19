using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Application.Models;

namespace Trycore.EVM.Application.Services;

public class EvmMetricsBuilder
{
    private readonly IEvmCalculationService _evmService;
    private readonly IEvmPerformanceInterpreter _interpreter;

    public EvmMetricsBuilder(
        IEvmCalculationService evmService,
        IEvmPerformanceInterpreter interpreter)
    {
        _evmService = evmService;
        _interpreter = interpreter;
    }

    public EvmActivityMetrics Build(
        decimal plannedProgressPercent,
        decimal actualProgressPercent,
        decimal budgetAtCompletion,
        decimal actualCost)
    {
        var pv = _evmService.CalculatePV(plannedProgressPercent, budgetAtCompletion);
        var ev = _evmService.CalculateEV(actualProgressPercent, budgetAtCompletion);
        var cv = _evmService.CalculateCV(ev, actualCost);
        var sv = _evmService.CalculateSV(ev, pv);
        var cpi = _evmService.CalculateCPI(ev, actualCost);
        var spi = _evmService.CalculateSPI(ev, pv);
        var eac = _evmService.CalculateEAC(budgetAtCompletion, cpi);
        var vac = _evmService.CalculateVAC(budgetAtCompletion, eac);

        return new EvmActivityMetrics
        {
            PV = pv,
            EV = ev,
            CV = cv,
            SV = sv,
            CPI = cpi,
            SPI = spi,
            EAC = eac,
            VAC = vac,
            CpiInterpretation = _interpreter.InterpretCpi(cpi, ev, actualCost),
            SpiInterpretation = _interpreter.InterpretSpi(spi, ev, pv)
        };
    }
}
