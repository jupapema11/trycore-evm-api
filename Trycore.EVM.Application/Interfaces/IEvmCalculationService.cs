namespace Trycore.EVM.Application.Interfaces;

public interface IEvmCalculationService
{
    decimal CalculatePV(decimal plannedPercent, decimal bac);

    decimal CalculateEV(decimal actualPercent, decimal bac);

    decimal CalculateCV(decimal ev, decimal ac);

    decimal CalculateSV(decimal ev, decimal pv);

    decimal CalculateCPI(decimal ev, decimal ac);

    decimal CalculateSPI(decimal ev, decimal pv);

    decimal CalculateEAC(decimal bac, decimal cpi);

    decimal CalculateVAC(decimal bac, decimal eac);
}