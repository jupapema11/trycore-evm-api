using Trycore.EVM.Application.Services;
using Xunit;

namespace Trycore.EVM.UnitTests.Services;

public class EvmCalculationServiceTests
{
    [Fact]
    public void CalculateCPI_WhenACIsZero_ShouldReturnZero()
    {
        var service = new EvmCalculationService();

        var result = service.CalculateCPI(1000, 0);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateSPI_WhenPVIsZero_ShouldReturnZero()
    {
        var service = new EvmCalculationService();

        var result = service.CalculateSPI(1000, 0);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateEAC_WhenCPIIsZero_ShouldReturnZero()
    {
        var service = new EvmCalculationService();

        var result = service.CalculateEAC(10000, 0);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculatePV_WhenBACIsZero_ShouldReturnZero()
    {
        var service = new EvmCalculationService();

        var result = service.CalculatePV(50, 0);

        Assert.Equal(0, result);
    }
}