using Trycore.EVM.Application.Services;
using Xunit;

namespace Trycore.EVM.UnitTests.Services;

public class EvmPerformanceInterpreterTests
{
    private readonly EvmPerformanceInterpreter _interpreter = new();

    [Fact]
    public void InterpretCpi_WhenGreaterThanOne_ReturnsUnderBudgetMessage()
    {
        var result = _interpreter.InterpretCpi(1.2m, 1200, 1000);

        Assert.Contains("Bajo presupuesto", result);
    }

    [Fact]
    public void InterpretCpi_WhenLessThanOne_ReturnsOverBudgetMessage()
    {
        var result = _interpreter.InterpretCpi(0.8m, 800, 1000);

        Assert.Contains("Sobre presupuesto", result);
    }

    [Fact]
    public void InterpretSpi_WhenGreaterThanOne_ReturnsAheadMessage()
    {
        var result = _interpreter.InterpretSpi(1.1m, 1100, 1000);

        Assert.Contains("Adelantado", result);
    }

    [Fact]
    public void InterpretSpi_WhenLessThanOne_ReturnsBehindMessage()
    {
        var result = _interpreter.InterpretSpi(0.9m, 900, 1000);

        Assert.Contains("Atrasado", result);
    }

    [Fact]
    public void InterpretCpi_WhenNoCostData_ReturnsNoDataMessage()
    {
        var result = _interpreter.InterpretCpi(0, 0, 0);

        Assert.Contains("Sin datos", result);
    }
}
