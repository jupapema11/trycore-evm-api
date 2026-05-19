using Trycore.EVM.Application.Services;
using Xunit;

namespace Trycore.EVM.UnitTests.Services;

public class EvmMetricsBuilderTests
{
    private readonly EvmMetricsBuilder _builder = new(
        new EvmCalculationService(),
        new EvmPerformanceInterpreter());

    [Fact]
    public void Build_WithSampleActivity_ReturnsCalculatedIndicators()
    {
        var metrics = _builder.Build(
            plannedProgressPercent: 50,
            actualProgressPercent: 40,
            budgetAtCompletion: 10000,
            actualCost: 5000);

        Assert.Equal(5000, metrics.PV);
        Assert.Equal(4000, metrics.EV);
        Assert.Equal(-1000, metrics.CV);
        Assert.Equal(-1000, metrics.SV);
        Assert.Equal(0.8m, metrics.CPI);
        Assert.Equal(0.8m, metrics.SPI);
        Assert.Equal(12500, metrics.EAC);
        Assert.Equal(-2500, metrics.VAC);
        Assert.False(string.IsNullOrWhiteSpace(metrics.CpiInterpretation));
        Assert.False(string.IsNullOrWhiteSpace(metrics.SpiInterpretation));
    }
}
