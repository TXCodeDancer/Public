using PensionComparison;

namespace PensionComparison.Tests;

public class PensionCalculatorTests
{
    [Fact]
    public void GetRetirementDate_AddsYearsAndMonthsFromBirthDate()
    {
        var birthDate = new DateOnly(1980, 1, 15);
        var plan = new PensionPlan("Plan A", 63, 9, 3000m);

        var result = PensionCalculator.GetRetirementDate(birthDate, plan);

        Assert.Equal(new DateOnly(2043, 10, 15), result);
    }

    [Fact]
    public void Compare_ReportsBreakEvenWhenCumulativePayoutsMatch()
    {
        var birthDate = new DateOnly(1960, 1, 1);
        var planA = new PensionPlan("Plan A", 63, 9, 3084m);
        var planB = new PensionPlan("Plan B", 67, 0, 3963m);
        var finalDate = new DateOnly(2045, 12, 31);

        var result = PensionCalculator.Compare(planA, planB, birthDate, finalDate);

        Assert.NotNull(result.BreakEvenDate);
        Assert.True(result.PlanATotalPayout > 0m);
        Assert.True(result.PlanBTotalPayout > 0m);
    }

    [Fact]
    public void Compare_ReturnsNoBreakEven_WhenOnePlanNeverCatchesUp()
    {
        var birthDate = new DateOnly(1980, 1, 1);
        var planA = new PensionPlan("Plan A", 65, 0, 1000m);
        var planB = new PensionPlan("Plan B", 65, 0, 2000m);
        var finalDate = new DateOnly(2045, 12, 31);

        var result = PensionCalculator.Compare(planA, planB, birthDate, finalDate);

        Assert.Null(result.BreakEvenDate);
    }
}
