using Trycore.EVM.Application.Constants;
using Trycore.EVM.Application.Interfaces;

namespace Trycore.EVM.Application.Services;

public class EvmPerformanceInterpreter : IEvmPerformanceInterpreter
{
    public string InterpretCpi(decimal cpi, decimal earnedValue, decimal actualCost)
    {
        if (earnedValue == 0 && actualCost == 0)
            return "Sin datos de costo";

        if (actualCost == 0)
            return "Sin costo real registrado";

        if (cpi > EvmConstants.OnTargetThreshold)
            return "Bajo presupuesto (eficiente en costos)";

        if (cpi < EvmConstants.OnTargetThreshold)
            return "Sobre presupuesto (gastando más de lo avanzado)";

        return "En presupuesto";
    }

    public string InterpretSpi(decimal spi, decimal earnedValue, decimal plannedValue)
    {
        if (earnedValue == 0 && plannedValue == 0)
            return "Sin datos de cronograma";

        if (plannedValue == 0)
            return "Sin valor planificado a la fecha";

        if (spi > EvmConstants.OnTargetThreshold)
            return "Adelantado al cronograma";

        if (spi < EvmConstants.OnTargetThreshold)
            return "Atrasado al cronograma";

        return "En cronograma";
    }
}
