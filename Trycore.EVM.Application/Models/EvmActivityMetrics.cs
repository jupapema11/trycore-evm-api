namespace Trycore.EVM.Application.Models;

public class EvmActivityMetrics
{
    public decimal PV { get; init; }

    public decimal EV { get; init; }

    public decimal CV { get; init; }

    public decimal SV { get; init; }

    public decimal CPI { get; init; }

    public decimal SPI { get; init; }

    public decimal EAC { get; init; }

    public decimal VAC { get; init; }

    public string CpiInterpretation { get; init; } = string.Empty;

    public string SpiInterpretation { get; init; } = string.Empty;
}
