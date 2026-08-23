namespace PensionComparison;

public sealed record PensionPlan(
    string Name,
    int RetirementAgeYears,
    int RetirementAgeMonths,
    decimal MonthlyPayout);
