namespace Trycore.EVM.Application.DTOs;

public class ActivityResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal BudgetAtCompletion { get; set; }

    public decimal PlannedProgressPercent { get; set; }

    public decimal ActualProgressPercent { get; set; }

    public decimal ActualCost { get; set; }

    public decimal PV { get; set; }

    public decimal EV { get; set; }

    public decimal CV { get; set; }

    public decimal SV { get; set; }

    public decimal CPI { get; set; }

    public decimal SPI { get; set; }

    public decimal EAC { get; set; }

    public decimal VAC { get; set; }

    public string CpiInterpretation { get; set; } = string.Empty;

    public string SpiInterpretation { get; set; } = string.Empty;
}
