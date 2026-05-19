using Trycore.EVM.Application.Constants;
using Trycore.EVM.Application.Interfaces;

namespace Trycore.EVM.Application.Services;

public class EvmCalculationService : IEvmCalculationService
{
    public decimal CalculatePV(decimal plannedPercent, decimal bac)
    {
        if (bac <= 0)
            return 0;

        return (plannedPercent / EvmConstants.PercentDivisor) * bac;
    }

    public decimal CalculateEV(decimal actualPercent, decimal bac)
    {
        if (bac <= 0)
            return 0;

        return (actualPercent / EvmConstants.PercentDivisor) * bac;
    }

    public decimal CalculateCV(decimal ev, decimal ac)
    {
        return ev - ac;
    }

    public decimal CalculateSV(decimal ev, decimal pv)
    {
        return ev - pv;
    }

    public decimal CalculateCPI(decimal ev, decimal ac)
    {
        if (ac == 0)
            return 0;

        return ev / ac;
    }

    public decimal CalculateSPI(decimal ev, decimal pv)
    {
        if (pv == 0)
            return 0;

        return ev / pv;
    }

    public decimal CalculateEAC(decimal bac, decimal cpi)
    {
        if (cpi == 0)
            return 0;

        return bac / cpi;
    }

    public decimal CalculateVAC(decimal bac, decimal eac)
    {
        return bac - eac;
    }
}