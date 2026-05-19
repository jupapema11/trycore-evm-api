using Trycore.EVM.Application.Services;
using Xunit;

namespace Trycore.EVM.UnitTests.Services;

public class EvmCalculationServiceTests
{
    private readonly EvmCalculationService _service = new();

    [Theory]
    [InlineData(50, 10000, 5000)]
    [InlineData(100, 8000, 8000)]
    public void CalculatePV_WithValidInputs_ReturnsExpectedValue(
        decimal plannedPercent,
        decimal bac,
        decimal expected)
    {
        var result = _service.CalculatePV(plannedPercent, bac);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(40, 10000, 4000)]
    [InlineData(0, 10000, 0)]
    public void CalculateEV_WithValidInputs_ReturnsExpectedValue(
        decimal actualPercent,
        decimal bac,
        decimal expected)
    {
        var result = _service.CalculateEV(actualPercent, bac);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateCV_ReturnsEarnedValueMinusActualCost()
    {
        Assert.Equal(500, _service.CalculateCV(1500, 1000));
    }

    [Fact]
    public void CalculateSV_ReturnsEarnedValueMinusPlannedValue()
    {
        Assert.Equal(-500, _service.CalculateSV(1500, 2000));
    }

    [Fact]
    public void CalculateCPI_WhenACIsZero_ShouldReturnZero()
    {
        Assert.Equal(0, _service.CalculateCPI(1000, 0));
    }

    [Fact]
    public void CalculateCPI_WhenValuesAreValid_ReturnsRatio()
    {
        Assert.Equal(1.25m, _service.CalculateCPI(5000, 4000));
    }

    [Fact]
    public void CalculateSPI_WhenPVIsZero_ShouldReturnZero()
    {
        Assert.Equal(0, _service.CalculateSPI(1000, 0));
    }

    [Fact]
    public void CalculateEAC_WhenCPIIsZero_ShouldReturnZero()
    {
        Assert.Equal(0, _service.CalculateEAC(10000, 0));
    }

    [Fact]
    public void CalculateEAC_WhenCPIIsValid_ReturnsEstimateAtCompletion()
    {
        Assert.Equal(8000, _service.CalculateEAC(10000, 1.25m));
    }

    [Fact]
    public void CalculateVAC_ReturnsBudgetMinusEac()
    {
        Assert.Equal(2000, _service.CalculateVAC(10000, 8000));
    }

    [Fact]
    public void CalculatePV_WhenBACIsZero_ShouldReturnZero()
    {
        Assert.Equal(0, _service.CalculatePV(50, 0));
    }

    [Fact]
    public void CalculateEV_WhenBACIsZero_ShouldReturnZero()
    {
        Assert.Equal(0, _service.CalculateEV(50, 0));
    }
}
