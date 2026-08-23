namespace PensionComparison;

public sealed record PensionComparisonResult(
    PensionPlan PlanA,
    PensionPlan PlanB,
    DateOnly BirthDate,
    DateOnly FinalPayoutDate,
    DateOnly PlanARetirementDate,
    DateOnly PlanBRetirementDate,
    decimal PlanATotalPayout,
    decimal PlanBTotalPayout,
    DateOnly? BreakEvenDate,
    int BreakEvenAgeYears,
    int BreakEvenAgeMonths);

public static class PensionCalculator
{
    public static DateOnly GetRetirementDate(DateOnly birthDate, PensionPlan plan)
    {
        return birthDate.AddYears(plan.RetirementAgeYears).AddMonths(plan.RetirementAgeMonths);
    }

    public static int CountMonthsBetween(DateOnly start, DateOnly end)
    {
        if (end < start)
            return 0;

        var months = ((end.Year - start.Year) * 12) + (end.Month - start.Month);

        if (end.Day < start.Day)
            months--;

        return Math.Max(months + 1, 0);
    }

    public static decimal CalculateTotalPayout(DateOnly retirementDate, DateOnly finalPayoutDate, decimal monthlyPayout)
    {
        if (finalPayoutDate < retirementDate)
            return 0m;

        return monthlyPayout * CountMonthsBetween(retirementDate, finalPayoutDate);
    }

    public static (int Years, int Months) GetAge(DateOnly birthDate, DateOnly targetDate)
    {
        var totalMonths = ((targetDate.Year - birthDate.Year) * 12) + (targetDate.Month - birthDate.Month);

        if (targetDate.Day < birthDate.Day)
            totalMonths--;

        var years = totalMonths / 12;
        var months = totalMonths % 12;

        return (years, months);
    }

    public static PensionComparisonResult Compare(PensionPlan planA, PensionPlan planB, DateOnly birthDate, DateOnly finalPayoutDate)
    {
        var planARetirementDate = GetRetirementDate(birthDate, planA);
        var planBRetirementDate = GetRetirementDate(birthDate, planB);

        var planATotalPayout = CalculateTotalPayout(planARetirementDate, finalPayoutDate, planA.MonthlyPayout);
        var planBTotalPayout = CalculateTotalPayout(planBRetirementDate, finalPayoutDate, planB.MonthlyPayout);

        DateOnly? breakEvenDate = null;
        var totalMonths = Math.Max(0, CountMonthsBetween(
            new DateOnly(Math.Min(planARetirementDate.Year, planBRetirementDate.Year), 1, 1),
            finalPayoutDate));

        decimal planAAccumulator = 0m;
        decimal planBAccumulator = 0m;

        var startDate = planARetirementDate <= planBRetirementDate ? planARetirementDate : planBRetirementDate;

        for (var currentDate = startDate; currentDate <= finalPayoutDate; currentDate = currentDate.AddMonths(1))
        {
            if (currentDate >= planARetirementDate)
                planAAccumulator += planA.MonthlyPayout;

            if (currentDate >= planBRetirementDate)
                planBAccumulator += planB.MonthlyPayout;

            if (breakEvenDate is null && planAAccumulator >= planBAccumulator && planAAccumulator > 0m && planBAccumulator > 0m)
            {
                breakEvenDate = currentDate;
            }
        }

        var breakEvenAgeYears = 0;
        var breakEvenAgeMonths = 0;

        if (breakEvenDate is not null)
        {
            var age = GetAge(birthDate, breakEvenDate.Value);
            breakEvenAgeYears = age.Years;
            breakEvenAgeMonths = age.Months;
        }

        return new PensionComparisonResult(
            planA,
            planB,
            birthDate,
            finalPayoutDate,
            planARetirementDate,
            planBRetirementDate,
            planATotalPayout,
            planBTotalPayout,
            breakEvenDate,
            breakEvenAgeYears,
            breakEvenAgeMonths);
    }
}
