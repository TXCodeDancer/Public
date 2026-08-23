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
    public void GetAgeDate_UsesYearsAndMonthsToComputeDate()
    {
        var birthDate = new DateOnly(1962, 8, 15);

        var result = PensionCalculator.GetAgeDate(birthDate, 65, 0);

        Assert.Equal(new DateOnly(2027, 8, 15), result);
    }

    [Fact]
    public void Compare_IsOrderIndependent_WhenPlansAreReversed()
    {
        var birthDate = new DateOnly(1962, 11, 18);
        var planA = new PensionPlan("Plan A", 67, 0, 3963m);
        var planB = new PensionPlan("Plan B", 70, 0, 4969m);
        var finalDate = new DateOnly(2050, 12, 31);

        var resultAFirst = PensionCalculator.Compare(planA, planB, birthDate, finalDate);
        var resultBFirst = PensionCalculator.Compare(planB, planA, birthDate, finalDate);

        Assert.Equal(resultAFirst.BreakEvenDate, resultBFirst.BreakEvenDate);
        Assert.Equal(resultAFirst.BreakEvenAmount, resultBFirst.BreakEvenAmount);
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
