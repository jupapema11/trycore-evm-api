namespace Trycore.EVM.Application.Interfaces;

public interface IEvmPerformanceInterpreter
{
    string InterpretCpi(decimal cpi, decimal earnedValue, decimal actualCost);

    string InterpretSpi(decimal spi, decimal earnedValue, decimal plannedValue);
}
